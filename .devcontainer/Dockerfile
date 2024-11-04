FROM mcr.microsoft.com/dotnet/sdk:8.0

USER root

RUN apt-get update && apt-get install -y \
    build-essential \
    curl \
    unzip \
    zip \
    git \
    sudo \
    && rm -rf /var/lib/apt/lists/*

RUN groupadd -g 1000 vscode \
    && useradd -u 1000 -g vscode -m vscode \
    && mkdir -p /home/vscode/.vscode-server \
    && chown -R vscode:vscode /home/vscode/.vscode-server

RUN echo "vscode ALL=(ALL) NOPASSWD:ALL" >> /etc/sudoers
RUN chsh -s /bin/bash vscode

USER vscode

RUN curl -fsSL https://fnm.vercel.app/install | bash
COPY .bashrc /home/vscode/
