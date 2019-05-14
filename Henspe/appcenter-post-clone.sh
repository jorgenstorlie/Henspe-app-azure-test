
#!/usr/bin/env bash
#
# Downloads the SNLA Core project so that it could be correctly linked at build time

myVar = $(pwd)
cd "../"

git clone https://github.com/snla-system/SNLA-app

cd $(myVar)