
#!/usr/bin/env bash
#
# Downloads the SNLA Core project so that it could be correctly linked at build time
git config --global --add url."git@github.com:".insteadOf "https://github.com/"
echo $(ls)
cd "../"
cd "../"
cd "../"
echo $(ls)

git clone https://github.com/snla-system/SNLA-app
