# Boilerplate .NET 10

Boilerplate profissional para .NET 10, construído com foco em arquitetura limpa, escalabilidade e nas melhores práticas do mercado.

O objetivo deste projeto é fornecer uma base completa para aplicações .NET, permitindo que eu comece a trabalhar nas features e regras de negócio imediatamente.

---

## Conceitos e Arquitetura

Este boilerplate implementa **Clean Architecture** combinada com **Vertical Slices**.

* **Clean Architecture:** Garante a separação de responsabilidades (Domain, Application, Infrastructure, Api) e a independência de frameworks.
* **Vertical Slices:** Em vez de organizar o código por tipo (ex: "Services", "Validators"), organizamos por *feature*. Isso mantém todo o código relacionado a uma funcionalidade (ex: "Criar Usuário") no mesmo local e fácil de manter.

## Principais Features

O projeto vem configurado com um conjunto de padrões e ferramentas modernas:

* **.NET 9:** Utilizando a versão mais recente do framework.
* **CQRS com MediatR:** Separação clara entre commands (escritas) e queries (leitura), administradas pelo MediatR.
* **MediatR Pipeline Behaviours:**
    * **`ValidationBehaviour`:** Valida automaticamente todos os commands e queries usando **FluentValidation** antes que cheguem aos *handlers*.
    * **`UnitOfWorkBehaviour`:** Gerencia transações de banco de dados de forma atômica para todos os *comandos*, garantindo que as operações de escrita estejam sempre encapsuladas em uma *transaction*.
* **Persistência com EF Core:**
    * **Unit of Work Pattern:** Abstrai o `DbContext` e gerencia as transações.
    * **Entidades Auditáveis (IAuditable):** Campos `CreatedAt` e `UpdatedAt` são preenchidos automaticamente.
* **Tratamento de Exceções Moderno:**
    * Uso de `IExceptionHandler` (a partir do .NET 8) para centralizar o tratamento de exceções de forma limpa.
* **Autenticação JWT:** Configuração base pronta para autenticação via JWT Bearer.

---

## Testes

O Projeto de testes (`Boilerplate.Application.Tests.Unit`) utiliza uma stack completa:

* **xUnit:** Framework de testes.
* **NSubstitute:** Para criação de mocks.
* **FluentAssertions:** Para asserts mais legíveis (ex: `resultado.Should().BeTrue()`).
* **Bogus:** Para geração de dados falsos consistentes.

---