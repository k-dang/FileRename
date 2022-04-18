#!/bin/bash

if [ $# -eq 0 ]
  then
    echo "Missing arguments"
fi

cd $1

filetypes=(".txt" ".html" ".png" ".jpg")
j=0

for i in $(seq $2); do
    newuuid=$(cat /dev/urandom | tr -dc "a-zA-Z0-9" | fold -w 8 | head -n 1)
    touch "$newuuid${filetypes[j]}"

    # increment
    ((j=j+1))
    if [[ "$j" -ge ${#filetypes[@]} ]]
    then
        j=0
    fi
done

echo List of Files:
ls -1 $1