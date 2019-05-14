
#!/usr/bin/env bash
#
# Downloads the SNLA Core project so that it could be correctly linked at build time

cd $APPCENTER_SOURCE_DIRECTORY

echo $(pwd)

git clone https://github.com/snla-system/SNLA-app