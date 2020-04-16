set PATH=C:\Program Files (x86)\Windows Kits\10\bin\10.0.18362.0\x64

makecert.exe ^
-n "CN=Allors Test Certificate" ^
-r ^
-pe ^
-a sha512 ^
-len 4096 ^
-cy authority ^
-sv ExcelAddIn.Interop_TemporaryKey.pvk ^
ExcelAddIn.Interop_TemporaryKey.cer
 
pvk2pfx.exe ^
-pvk ExcelAddIn.Interop_TemporaryKey.pvk ^
-spc ExcelAddIn.Interop_TemporaryKey.cer ^
-pfx ExcelAddIn.Interop_TemporaryKey.pfx

del ExcelAddIn.Interop_TemporaryKey.pvk
del ExcelAddIn.Interop_TemporaryKey.cer