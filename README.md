# Serviços de Renda Variável
[![Build and Tests](https://github.com/davypaulino/finrv/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/davypaulino/finrv/actions/workflows/build-and-test.yml)
[![CodeQL Advanced](https://github.com/davypaulino/finrv/actions/workflows/codeql.yml/badge.svg)](https://github.com/davypaulino/finrv/actions/workflows/codeql.yml)

### [Documentação da API](https://localhost:7578/api-reference)

### Arquitetura do serviço de renda variável (Container C4 Model)

![Diagrama de Aplicação Financeira C4 Model Container](./doc/container.svg)
<details>
    <summary>Diagram as Code (PlantUML)</summary>
    <pre>
    @startuml
    <!-- !include https://raw.githubusercontent.com/plantuml-stdlib/C4-PlantUML/master/C4_Container.puml -->

    title Aplicação Financeira

    Container_Boundary(s1, "Sistema de Renda Variável") {
        Container(web, "Web Page", "Javascript, Tailwind", "Interface para ações financeiras")
        Container(so, "Web API", ".NET 9", "Serviços para operações financeiras")
        Container(wsc, "Worker Service", ".NET 9", "Serviços para recepção de cotações")
        Container(database, "Database", "MySQL")
    }

    Container_Ext(monitoring, "Observabilidade", "Prometheus, Grafana")
    Container_Ext(kafka, "Fila de Mensageria", "Kafka")

    Rel(web, so, "Consome")
    Rel(so, database, "Insere e consome")
    Rel(wsc, database, "Insere e consome")
    Rel(wsc, kafka, "Consome")
    Rel(wsc, monitoring, "Envia Logs e Métricas")
    Rel(so, monitoring, "Envia Logs e Métricas")

    @enduml
    </pre>
</details>


Claro! Aqui está uma versão aprimorada da sua documentação, com correções, mais clareza e melhor organização:

---

# 📌 Como Executar  
Guia para configurar e rodar a aplicação localmente.

___

## 🚀 **Pré-requisitos**
Certifique-se de ter os seguintes itens instalados antes de iniciar:  

- [`.NET 9`](https://dotnet.microsoft.com/)  
- [`Entity Framework Core 9`](https://learn.microsoft.com/en-us/ef/core/)  
- [`Podman`](https://podman.io/) ou [`Docker`](https://www.docker.com/)  
- *Imagems para containers:*
	- `mysql:8`  

___

## ⚙️ **Configuração de Variáveis de Ambiente**  
Antes de executar a aplicação, configure suas variáveis de ambiente.  

- 1️⃣ Copie o arquivo `.env-example` e renomeie para `.env`.  
- 2️⃣ Edite o `.env` e ajuste os valores conforme necessário.
- 🔹 O arquivo `.env` deve estar na raiz do projeto para que o `docker-compose.yml` consiga encontrá-lo.

___

## 🛠️ **Executando o Banco de Dados**  
Use os seguintes comandos para iniciar os containers do MySQL:

🔹 *Com Docker:* 
```bash
docker-compose up -d
```
🔹 *Com Podman:* 
```bash
podman-compose up -d
```
> O parâmetro `-d` mantém o container rodando em segundo plano.

___

## 📌 **Aplicando as Migrations**  
Se for necessário aplicar **migrations** no banco de dados, use os comandos abaixo.

🔹 *Via Package Manager Console (PMC)*  
> Executar dentro do projeto `ApiService`  
```powershell
Update-Database -Project finrv.Infra
```

🔹 *Via Linha de Comando (`dotnet ef` CLI)*  
> Executar dentro do projeto `ApiService`  
```bash
dotnet ef database update --project finrv.Infra
```
