# Projeto do Ledes Games

Este repositório armazena o projeto de jogo de plataforma 3d de GameJam do processo seletivo para compor o time do Ledes Games. O jogo será desenvolvido utilizando unity e seu tema será sobre coleta e reciclagem de lixo combinado com um estilo visual inspirado em Doom.

## Sumário

- [Pré-requisitos](#requisites)
- [Como abrir este projeto ?](#openproject)
- [Como contribuir com este projeto ?](#contribute)

<h2 id="requisites">Pré-requisitos</h2>

Este projeto utiliza Unity Engine versão 6.3 LTS e Git.

### Instalação do Unity Hub e da Unity Engine

- [Baixe o Unity Hub aqui.](https://cloud.unity.com/home/organizations/4674163843771/onboarding/post-download?locale=en&code=NvlLw5ttJP7N0p0feAWWKQ004f&locale=en&session_state=4e32d152cd081cc74a003875a3dbc170f3887f60fd53c73dc6f365c85e18562b.6nH60vL1vLziRJcWuLijNQ001f)
- Execute o instalador e siga as instruções.
- Uma vez que o Unity Hub está instalado, a plataforma irá te guiar para a instalação do editor (Certifique-se de instalar a versão 6.3 LTS).

Obs : Caso o processo de instalação não te guie para a instalação do editor, abra a aba 'Resources' -> 'Install Editor' e selecione a versão 6.3 LTS.

### Instalação do Git

- [Baixe o Git aqui.](https://git-scm.com/install/windows)
- Execute o instalador e siga as instruções

Obs : Se for a primeira vez que você vai usar o git no seu computador, então será necessário definir seu nome e email com os comandos : 

- <code>git config --global user.name "Your Name"</code> 
- <code>git config --global user.email "your.email@example.com"</code>

<h2 id="openproject"> Como abrir este projeto ? </>

### Clone o repositório

- Execute : <code>git clone https://github.com/Erick-P-Calauro/doom-ledes-games [nome da pasta local] </code>

### Importe o projeto no Unity Hub

- Abra o Unity Hub.
- Vá até a aba de Projetos ('Projects').
- No canto superior direito, clique em add -> add project from disk.
- Procure a pasta com o clone do repositório do projeto.

Ao fim, você terá o projeto do jogo em sua lista de projetos no unity hub, sendo possível fazer as alterações necessárias.

<h2 id="contribute">Como contribuir com este projeto ?</h2>

Após fazer as alterações necessárias no projeto, utilizando a Unity Engine e seu editor de código preferido, execute os seguintes passos para enviar as novas features ao repositório : 

### Crie um commit : 

Para criar um commit com as alterações feitas : 
- Crie um repositório local (Caso ainda não tenha feito) :  <code>git init</code>
- Adicione todas as alterações : <code>git add *</code>
- Selecione/Crie sua branch pessoal : <code>git branch -M [sua-branch] </code>
- Execute o commit : <code>git commit -m [descrição do seu commit]</code>

IMPORTANTE : Sempre que fizer um novo commit, procure seguir os padrões de descrição contidos em [Padrões de Commit](https://github.com/iuricode/padroes-de-commits).

Observação : Nessa etapa o conteúdo de suas alterações ainda está restrito ao seu computador. Contudo você consegue ver a árvore de alterações com o comando : <code>git log</code>

### Atualize a sua branch com o novo commit : 

A atualização da branch pode ser feita por meio do link do repositório ou por meio de uma origem remota. Este passo a passo vai seguir o procedimento da origem remota pois facilita o procedimento de merge.

- Adicione a origem remota do repositório : <code>git remote add origin https://github.com/Erick-P-Calauro/doom-ledes-games</code>
- Execute : <code>git push origin [sua-branch]</code>

Observação : O nome da origem remota 'origin' é customizável.

### Execute o merge do conteúdo na master :

O processo de fazer o 'merge' do conteúdo na master adiciona o conteúdo da sua branch pessoal ao histórico de conteúdo do  branch master. Como a branch master deve ser o ramo principal e referência de versão mais recente, procure ter certeza que suas alterações não contém erros graves.

- Baixe a versão mais recente da master : <code>git fetch origin master</code>
- Baixe a versão mais recente da sua branch : <code>git fetch origin [sua-branch]</code>
- Execute :   <code>git checkout master</code>
- Faça o merge : <code> git merge [sua-branch]</code>
- Caso tudo tenha ocorrido conforme planejado, upe as alterações na branch master.

Observações : 
- É possível que ocorram conflitos durante o processo de merge. [Tutorial de como  resolver merge conflicts](https://www.geeksforgeeks.org/git/merge-conflicts-and-how-to-handle-them/).
- Sempre que possível, atualize o estado da sua branch pessoal para se equiparar com estado da branch  master.
- Evite fazer commits direto na branch master sempre que possível.