# Gerenciador de Produtos

Projeto desenvolvido como parte do desafio final do **Bootcamp Squadra Digital**, com foco em **C# e .NET 8**. Este sistema é capaz de gerenciar produtos, oferecendo funcionalidades como cadastro, edição, exclusão, autenticação de usuários e integração com um banco de dados SQL Server.

## Funcionalidades

- **Cadastro de Produtos**: Adicionar, editar e remover produtos do sistema.
- **Listagem de Produtos**: Consultar produtos cadastrados no banco de dados.
- **Autenticação**: Sistema de login utilizando JWT (JSON Web Tokens) para segurança.
- **Banco de Dados**: Persistência de dados utilizando SQL Server.

## Tecnologias Utilizadas

- **Linguagem**: C#
- **Framework**: .NET 8
- **Banco de Dados**: SQL Server (via SQL Server Management Studio - SSMS)
- **Autenticação**: JWT (JSON Web Tokens)
- **IDE**: Visual Studio
- **Controle de Versão**: Git e GitHub

## Requisitos para Execução

1. **Ambiente de Desenvolvimento**:
   - Visual Studio (2022 ou superior)
   - .NET SDK 8 instalado
   - SQL Server Management Studio (SSMS)

2. **Banco de Dados**:
   - Configure o banco de dados utilizando o **SQL Server Management Studio**.
   - Importe ou crie as tabelas necessárias para o projeto (consulte a documentação para mais detalhes).

3. **Configuração da String de Conexão**:
   Atualize a string de conexão no arquivo `appsettings.json` com as credenciais corretas para o servidor SQL Server. Exemplo:

   ```json
   "ConnectionStrings": {
     "GerenciadorConnection": "Server=SEU_SERVIDOR;Database=GerenciadorProdutos;Trusted_Connection=True;"
   }
