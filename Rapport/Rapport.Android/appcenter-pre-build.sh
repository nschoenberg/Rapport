#!/usr/bin/env bash
#
# Restores secrets.txt during app center build

if [ -z "$SECRETS_CONTENT" ]
then
    echo "You need define the $SECRETS_CONTENT variable in App Center"
    exit
fi

SECRETS_FILE=$APPCENTER_SOURCE_DIRECTORY/Rapport/Rapport/Resources/secrets.txt


echo $SECRETS_CONTENT > SECRETS_FILE
echo "File content:"
cat SECRETS_FILE
