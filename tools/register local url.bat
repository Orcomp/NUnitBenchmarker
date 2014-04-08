:: if \Everyone does not work, please specify a DOMAIN
:: netsh http delete urlacl url=http://+:12345/
:: netsh http add urlacl url=http://+:12345/ user=Everyone

netsh http add urlacl url=http://+:8091/NUnitBenchmarker/UIService/ user=Everyone

pause