# Projeto: Web API com ASP.NET Core e Entity Framework (Atualizado para o .NET 5.0)

Adaptação para WEB API do projeto pertencente a **Seção 18** do '**Curso C# COMPLETO 2020 Programação Orientada a Objetos + Projetos**'.

Instrutor:

- [Nélio Alves](https://www.udemy.com/user/nelio-alves/)

Referências:

- https://www.udemy.com/course/programacao-orientada-a-objetos-csharp/
- https://github.com/jeanbarcellos/estudo.csharp.nelioalves.sales
- https://github.com/jeanbarcellos/estudo.csharp.nelioalves.sales-api-net2.1

## Anotações

Instalação de pacotes:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

dotnet add package Microsoft.EntityFrameworkCore.Design
```

Migrações:

```bash
dotnet ef migrations add Initials
dotnet ef database update
```

### Formas de usar o método Created() no retorno do sucesso do POST

Geralmente, após o sucesso da utilização do POST, um objeto é fornecido no corpo do response, juntamente com um header Location contendo a URL do produto recém-criado.
Isso pode ser feito das seguintes maneiras:

Forma 1

```cs
return CreatedAtAction(nameof(Details), new { id = department.Id }, department);
```

Forma 2

```cs
return Created(new Uri(Url.Link("Details", new { id = department.Id })), department);
```

Forma 3 - Retornando apenas o ID

```cs
return Created(department.Id.ToString(), department);
```

## TO-DO

- Resolver recursão nos retornos JSON
- Resolver problema no group GroupingSearch em SalesRecordsController
- Verificar retornos dos controllers
