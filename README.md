# Customer Management API

## Sobre o projeto

Esta Web API possui o objetivo de registrar dados básicos de clientes (nome, email e telefones) com propósitos meramente didáticos. 

## Requisitos técnicos

Esta API possui as seguintes funcionalidades:

1. Cadastrar o cliente informando o nome completo, e-mail e uma lista de telefones informando o DDD, número e o tipo (fixo ou celular);
2. Permitir consultar todos os clientes com seus respectivos e-mails e telefones;
3. Permitir a consulta de um cliente através do DDD e número;
4. Permitir a atualização do e-mail do cliente cadastrado;
5. Permitir a atualização do telefone do cliente cadastrado;
6. Permitir a exclusão de um cliente através do e-mail.

## Tecnologias e bibliotecas utilizadas no projeto

- .NET Core 6
- Microsoft SQL Server - LocalDB
- xUnit
- AutoMapper
- EntityFramework Core

## Estrutura da Aplicação

|   **Projeto** | **Objetivo**  |
|---------------|----------------|
| **CustomerManagement**               | Projeto principal, aqui estão localizados os endpoints das APIs. |
| **CustomerManagement.Business**      | Este projeto fornece a lógica da aplicação, validações e comunicação entre os Controllers e as camadas do repositório (banco de dados). |
| **CustomerManagement.Repository**          | This project provides an abstraction of the database connection and CRUD (Create, Retrieve, Update and Delete) operations. |
| **CustomerManagement.Models**              | Esta biblioteca de classes contém os objetos das entidades e definições das tabelas do banco de dados. |
| **CustomerManagement.ViewModels**          | Esta biblioteca de classes contém os objetos POCO retornados pelas camadas de negócios e API aos clientes. |
| **CustomerManagement.Infra.Core**          | Esta biblioteca de classes contém principalmente auxiliares, constantes e classes comuns do projeto. |
| **CustomerManagement.Infra.AutoMapper**          | Esta biblioteca de classes contém as definições da conversão entre objetos utilizando a biblioteca [AutoMapper](https://automapper.org) |
| **CustomerManagement.Test**                  | Testes unitários do projeto *CustomerManagement*  |
| **CustomerManagement.Business.Test**         | Testes unitários do projeto *CustomerManagement.Business*  |
| **CustomerManagement.Infra.Core.Test**       | Testes unitários do projeto *CustomerManagement.Infra.Core*  |

## Executando a aplicação

Antes de iniciar o aplicativo pela primeira vez, verifique a string de conexão do banco de dados localizada no arquivo ***appsettings.json*** e altere-a se necessário. O banco de dados e todas as migrações pendentes são aplicadas automaticamente durante a inicialização do aplicativo.

Após iniciar a depuração, uma nova janela do navegador será aberta com a documentação do Swagger da API. Você pode testar todos os endpoints por meio dele ou pela *collection* do Postman disponível aqui.

[<img src="https://run.pstmn.io/button.svg" alt="Run In Postman" style="width: 128px; height: 32px;">](https://god.gw.postman.com/run-collection/11264835-c2cf8248-de4c-486d-bc79-e93b045eabd5?action=collection%2Ffork&source=rip_markdown&collection-url=entityId%3D11264835-c2cf8248-de4c-486d-bc79-e93b045eabd5%26entityType%3Dcollection%26workspaceId%3Dbf15e266-2224-46d2-a7a9-3fafc1bc1a17)

**P.S.:**: O arquivo da collection do Postman também está disponível neste projeto para execução local.