#!/bin/bash
scriptname=$(basename "$0")
sourcedb=$(pwd)/${1}
option=${@:2} 

RED="\033[31m"
YELLOW="\033[33m"
GREEN="\033[32m"
RESET="\033[0m"

print_yellow() {
    echo -e "${YELLOW}${1}${RESET}"
}

print_red() {
    echo -e "${RED}${1}${RESET}"
}

print_green() {
    echo -e "${GREEN}${1}${RESET}"
}

if [ "$#" -lt 1 ]; then
    print_yellow "\nPlease provide relative path of codeql database directory"
    print_yellow "\nUsage: ${scriptname} <directory containing source_db>"
    print_yellow "\nExample: ${scriptname} ./results"
   exit 1
fi

print_yellow "\nPull latest planetql image"
docker pull soohoio/planetql:latest
if [ $? -eq 0 ]
then
    print_green "Image pull success\n"
else
    print_red "Image pull failed\n"
    exit 1
fi

print_yellow "\nRunning query"
docker run --rm --name planetql -v "${sourcedb}:/opt/results" soohoio/planetql:latest planetql analyze ${option}
if [ $? -eq 0 ]
then
    print_green "Query run success\n"
else
    print_red "Query run failed\n"
fi
