#!/bin/bash
scriptname=$(basename "$0")
inputfile=$(pwd)/${1}
outputfile=$(pwd)/${2}

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

if [ "$#" -ne 2 ]; then
    print_yellow "\nPlease provide project folder to create database and folder to store created database."
    print_yellow "\nUsage: ${scriptname} <project folder> <output folder>"
    print_yellow "\nExample: ${scriptname} ./project ./results"
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

print_yellow "Creating the codeQL database. This might take some time depending on the size of the project...\n"
docker run --rm --name planetql -v "${inputfile}:/opt/src" -v "${outputfile}:/opt/results" soohoio/planetql:latest planetql setup
if [ $? -eq 0 ]
then
    print_green "Successfully created the database\n"
else
    print_red "Failed to create the database\n"
    exit 1
fi
