# Modelagem de Entidade Usuário

| Nome | Nome no Banco | Tipo | Justificativa |
| ---  | ---           |---   |   ----        |
| Id | `id` | guid | Seguranção com identificação unico | 

Claro! Aqui está uma versão mais detalhada e aprimorada da tabela, incluindo os possíveis tipos de dados e justificativas para cada campo:

### 📌 **Tabela aprimorada**
| Nome             | Nome no Banco   | Tipo          | Justificativa |
|-----------------|----------------|--------------|--------------|
| Id              | `id`            | `GUID`       | O GUID garante IDs únicos e seguros, evitando conflitos de identificação. Além disso, dificulta previsibilidade e melhora a escalabilidade em grandes bancos de dados. |
| Nome            | `nome`          | `VARCHAR(100)` | Limitar o o consumo de memoria limitando o numero de caracteres para o registro. |
| Email           | `email`         | `VARCHAR(150)` | . |
| % Corretagem    | `porcentagem_corretagem` | `DECIMAL(10,2)` | Representa a taxa de corretagem, garantindo precisão nos cálculos financeiros. |
| Criado em       | `criado_em`     | `DATETIME(6)` | Registra quando o usuário foi criado, útil para auditoria. |
| Criado por      | `criado_por`    | `VARCHAR(100)` | Guarda quem criou o registro. |
| Atualizado em   | `atualizado_em` | `DATETIME(6)` | Permite rastrear a última atualização do registro. |
| Atualizado por  | `atualizado_por` | `VARCHAR(100)` | Registra quem fez a última alteração. |


Usuários (Id, Nome, Email, %Corretagem)
 Ativos (Id, Codigo, Nome)
 Operações (Id, UsuarioId, AtivoId, Quantidade, PrecoUnitario, TipoOperacao,
Corretagem, DataHora)
 Cotação: (Id, AtivoId, PrecoUnitario, DataHora)
 Posição: (Id, IdUsuario, AtivoId, Quantidade, PrecoMedio, P&amp;L)