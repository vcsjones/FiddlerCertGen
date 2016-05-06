Name "FiddlerCertGenerator4"

!define /date YEAR "%Y"

SetCompressionLevel 9
RequestExecutionLevel "user"

BrandingText "v${VERSION}"
VIProductVersion "${VERSION}"
VIAddVersionKey "ProductVersion" "${VERSION}"
VIAddVersionKey "FileVersion" "${VERSION}"
VIAddVersionKey "ProductName" "${ProductName}"
VIAddVersionKey "Comments" "${ProductUrl}"
VIAddVersionKey "LegalCopyright" "©${YEAR} ${ProductCopy}"
VIAddVersionKey "CompanyName" "${CompanyName}"
VIAddVersionKey "FileDescription" "${ProductDescription}"

InstallDir "$DOCUMENTS\Fiddler2"

Section "Main"
SetOverwrite on
SetOutPath "$INSTDIR\Scripts"
File "..\out\VCSJones.FiddlerCertProvider4.dll"
File "..\out\VCSJones.FiddlerCertProvider4.pdb"
File "..\out\VCSJones.FiddlerCertGen.dll"
File "..\out\VCSJones.FiddlerCertGen.pdb"

MessageBox MB_OK "YAY! Go generate some certificates!"

SectionEnd