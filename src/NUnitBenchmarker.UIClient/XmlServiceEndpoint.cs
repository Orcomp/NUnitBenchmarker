using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace NUnitBenchmarker
{
    /// <summary>
    ///     Class XmlServiceEndpoint for configuring WCF manually
    /// </summary>
    public class XmlServiceEndpoint : ServiceEndpoint
    {
        #region Constants and Fields

        /// <summary>
        ///     The configuration instance
        /// </summary>
        private readonly Configuration configuration;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlServiceEndpoint" /> class.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <param name="configXml">The service model XML.</param>
        /// <param name="endpointName">Name of the endpoint.</param>
        public XmlServiceEndpoint(Type contractType, string configXml, string endpointName)
            : this(ContractDescription.GetContract(contractType), configXml, endpointName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlServiceEndpoint" /> class.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="configXml">The service model XML.</param>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">Invalid configXml.</exception>
        public XmlServiceEndpoint(ContractDescription contract, string configXml, string endpointName)
            : base(contract)
        {
            FileInfo configurationFile = null;
            try
            {
                if (string.IsNullOrWhiteSpace(configXml))
                {
                    throw new ArgumentNullException();
                }

                string configFilePath = Path.GetTempFileName();
                configurationFile = new FileInfo(configFilePath);

                using (FileStream stream = configurationFile.Create())
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(configXml);
                    configurationFile.IsReadOnly = true;
                }

                var map = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = configFilePath
                };

                configuration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

                if (ServiceModel == null)
                {
                    throw new ArgumentException("Invalid configuration.");
                }

                ChannelEndpointElement endpoint = null;
                foreach (
                    ChannelEndpointElement end in
                        ServiceModel.Client.Endpoints.Cast<ChannelEndpointElement>().Where(end => end.Name == endpointName))
                {
                    endpoint = end;
                }

                if (endpoint == null)
                {
                    throw new ArgumentException("Invalid endpointName \"" + endpointName + "\".");
                }
                if (endpoint.Address == null)
                {
                    throw new ArgumentException("Invalid endpoint address.");
                }

                Address = new EndpointAddress(endpoint.Address);

                SetBinding(endpoint.BindingConfiguration, endpoint.Binding);
                SetBehaviours();
            }
            finally
            {
                try
                {
                    if (configurationFile != null && configurationFile.IsReadOnly)
                    {
                        configurationFile.IsReadOnly = false;
                    }
                }
                catch
                {
                }
                DeleteConfigFile();
            }
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="XmlServiceEndpoint" /> class.
        /// </summary>
        ~XmlServiceEndpoint()
        {
            DeleteConfigFile();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the service model.
        /// </summary>
        /// <value>The service model.</value>
        private ServiceModelSectionGroup ServiceModel
        {
            get
            {
                try
                {
                    return ServiceModelSectionGroup.GetSectionGroup(configuration);
                }
                catch
                {
                    return null;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Deletes the configuration file.
        /// </summary>
        private void DeleteConfigFile()
        {
            try
            {
                if (configuration != null && !string.IsNullOrWhiteSpace(configuration.FilePath))
                {
                    File.Delete(configuration.FilePath);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        ///     Sets the behaviours.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Unexpected error creating client endpoint behaviors</exception>
        private void SetBehaviours()
        {
            if (ServiceModel == null || ServiceModel.Behaviors == null || ServiceModel.Behaviors.EndpointBehaviors == null)
            {
                return;
            }
            try
            {
                foreach (IEndpointBehavior b in from EndpointBehaviorElement behavior in ServiceModel.Behaviors.EndpointBehaviors
                                                where behavior != null
                                                from behaviorExtension in behavior.Where(b => b != null)
                                                let createBehavior = behaviorExtension.GetType()
                                                    .GetMethod(
                                                        "CreateBehavior",
                                                        BindingFlags.NonPublic
                                                        | BindingFlags.Instance)
                                                select (IEndpointBehavior)createBehavior.Invoke(behaviorExtension, new object[0])
                                                    into b
                                                    where b != null
                                                    select b)
                {
                    Behaviors.Add(b);
                }
            }
            catch (Exception ee)
            {
                throw new InvalidOperationException("Unexpected error creating client endpoint behaviors", ee);
            }
        }

        /// <summary>
        ///     Sets the binding information.
        /// </summary>
        /// <param name="bindingName">Name of the binding.</param>
        /// <param name="bindingType">Type of the binding.</param>
        /// <exception cref="System.ArgumentNullException">Invalid binding name;bindingName</exception>
        /// <exception cref="System.ArgumentException">Invalid configXml.</exception>
        /// <exception cref="System.Exception">
        ///     Invalid binding configuration, no bindings of type  + bce.BindingType.Name
        ///     +  with a parameterless constructor were found
        /// </exception>
        private void SetBinding(string bindingName, string bindingType)
        {
            if (string.IsNullOrWhiteSpace(bindingName))
            {
                throw new ArgumentNullException("Invalid binding name", "bindingName");
            }
            if (string.IsNullOrWhiteSpace(bindingType))
            {
                throw new ArgumentNullException("Invalid binding type", "bindingType");
            }
            if (ServiceModel == null || ServiceModel.Bindings == null)
            {
                throw new ArgumentException("Invalid configXml.");
            }

            foreach (BindingCollectionElement bce in ServiceModel.Bindings.BindingCollections.Where(b => b != null))
            {
                foreach (
                    IBindingConfigurationElement bind in
                        bce.ConfiguredBindings.Where(b => b != null)
                            .Where(bind => bind.Name == bindingName && bce.BindingName == bindingType))
                {
                    try
                    {
                        Binding = (Binding)bce.BindingType.GetConstructor(new Type[0]).Invoke(new object[0]);

                        bind.ApplyConfiguration(Binding);

                        Binding.Name = bind.Name;
                        return;
                    }
                    catch (NullReferenceException)
                    {
                        throw new Exception(
                            "Invalid binding configuration, no bindings of type " + bce.BindingType.Name
                            + " with a parameterless constructor were found");
                    }
                    catch (Exception ee)
                    {
                        throw new Exception("Invalid binding configuration.\r\n" + ee.Message, ee);
                    }
                }
            }

            throw new Exception("Invalid binding configuration, binding \"" + bindingName + "\" was not found.");
        }

        #endregion
    }
}
