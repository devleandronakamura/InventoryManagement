# Inventário Management API

Este é um projeto de gerenciamento de inventário, onde o banco de dados MySQL foi utilizado para armazenar as informações. Abaixo estão as instruções para configurar o banco de dados e a conexão.

## Banco de Dados

O projeto utiliza o **MySQL** como banco de dados para persistência de dados.

### Criando o Banco de Dados

Para criar o banco de dados e as tabelas necessárias, execute o script `SeedSql.sql` incluído no repositório. Esse arquivo contém os comandos SQL necessários para a criação do banco de dados `InventoryDb` e suas tabelas.

### Configuração da Conexão

É necessário alterar a string de conexão no arquivo `appsettings.json` dentro do projeto `InventoryManagement.Api` para refletir as configurações do seu ambiente local.

No arquivo `appsettings.json`, localize a seção `ConnectionStrings` e altere a string de conexão conforme abaixo:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=InventoryDb;User=root;Password=suaSenha;"
}
