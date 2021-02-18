FROM mcr.microsoft.com/dotnet/sdk:3.1-buster AS dotnet_sdk

LABEL maintainer="Sooho <angjin@sooho.io>"

# tzdata install needs to be non-interactive
ENV DEBIAN_FRONTEND=noninteractive

# install/update basics 
RUN apt-get update && \
    apt-get upgrade -y && \
    apt-get install -y --no-install-recommends \
   	wget \
   	git \
   	unzip \
    npm && \
    apt-get clean 

# Install latest CodeQL
ENV CODEQL_HOME /usr/local/codeql-home
RUN mkdir -p ${CODEQL_HOME} \
    ${CODEQL_HOME}/codeql-repo 

# CodeQL CLI
ENV CODEQL_VERSION v2.4.3
RUN wget -q https://github.com/github/codeql-cli-binaries/releases/download/${CODEQL_VERSION}/codeql-linux64.zip -O /tmp/codeql_linux.zip && \
    unzip /tmp/codeql_linux.zip -d ${CODEQL_HOME} && \
    rm /tmp/codeql_linux.zip && \
    rm -rf `find ${CODEQL_HOME}/codeql -mindepth 1 -maxdepth 1 -not -path '*/\.*' | grep -v -E 'csharp$|tools$|codeql$|xml$' `


# Clone csharp qlpack and remain only csharp qlpack
RUN git clone https://github.com/github/codeql ${CODEQL_HOME}/codeql-repo && \ 
    rm -rf `find ${CODEQL_HOME}/codeql-repo -mindepth 1 -maxdepth 1 -not -path '*/\.*' | grep -v -E 'csharp$' `

ENV PATH="${CODEQL_HOME}/codeql:${PATH}"

# Copy analyze queries
COPY queries/ /opt/ql/

# Install planetQL CLI
RUN npm install -g planetql 

CMD [ "planetql" , "help"]
