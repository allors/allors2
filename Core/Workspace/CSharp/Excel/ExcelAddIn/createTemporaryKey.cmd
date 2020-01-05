set PATH=C:\Program Files (x86)\Windows Kits\10\bin\10.0.17763.0\x64

makecert.exe ^
-n "CN=Allors Test Certificate" ^
-r ^
-pe ^
-a sha512 ^
-len 4096 ^
-cy authority ^
-sv ExcelAddIn_TemporaryKey.pvk ^
ExcelAddIn_TemporaryKey.cer
 
pvk2pfx.exe ^
-pvk ExcelAddIn_TemporaryKey.pvk ^
-spc ExcelAddIn_TemporaryKey.cer ^
-pfx ExcelAddIn_TemporaryKey.pfx

del ExcelAddIn_TemporaryKey.pvk
del ExcelAddIn_TemporaryKey.cer