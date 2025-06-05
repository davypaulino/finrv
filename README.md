# Serviços de Renda Variável
Arquitetura do serviço de renda variável.

```mermaid
C4Container
title fin aplication

Container_Boundary(s1, "Sistema de Renda Variavel") {
   Container(web, "web page", "Javascript, Tailwind", "Interface para para ações financeiras")
   Container(so, "Web Api", ".Net 9", "Serviços para operações financeiras")
   Container(wsc, "Worker Service", ".Net 9", "Serviços para recepção de cotações")
   ContainerDb(database, "Database", "MySql")

}
Container_Ext(monitoring, "Observabilidade", "Prometheus, Grafana")
Container_Ext(kafka, "Fila de Messageria", "Kafka")

Rel(web, so, "Consome")
Rel(so, database, "Insere e consome")
Rel(wsc, database, "Insere e consome")
Rel(wsc, kafka, "consome")
Rel(wsc, monitoring, "Envia Log e metricas")
Rel(so, monitoring, "Envia Log e metricas")
```

# Como Executar
Instruções para executar aplicação localmente.

## Configurações de variáveis de ambiente
Use o arquivo `.env-example` para criar seu arquivo `.env` com suas variaveis de ambiente.
