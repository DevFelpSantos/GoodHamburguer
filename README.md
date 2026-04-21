# 🍔 GoodHamburger API

**Sistema de Gerenciamento de Pedidos de Hambúrgueres com Descontos Inteligentes**

Uma API RESTful robusta e bem arquitetada para gerenciar pedidos de hambúrgueres com cálculo automático de descontos baseado em regras de negócio.

---

## 📋 Sumário

- [Visão Geral](#visão-geral)
- [Stack Tecnológico](#stack-tecnológico)
- [Arquitetura](#arquitetura)
- [Decisões de Projeto](#decisões-de-projeto)
- [Como Rodar](#como-rodar)
- [Endpoints](#endpoints)
- [Regras de Negócio](#regras-de-negócio)
- [Testes](#testes)
- [Estrutura do Projeto](#estrutura-do-projeto)

---

## 👁️ Visão Geral

GoodHamburger é uma API moderna para gerenciar restaurantes de hambúrgueres, com foco em:

✅ **Cardápio dinâmico** - Gerenciar itens de menu (sanduíches, acompanhamentos, bebidas)
✅ **Pedidos inteligentes** - Criar e rastrear pedidos com validações automáticas
✅ **Descontos automáticos** - Cálculo inteligente de descontos baseado em combinações de itens
✅ **Documentação interativa** - Swagger UI integrado para explorar endpoints
✅ **Testes completos** - Unit tests, integration tests e cobertura de código
✅ **Docker ready** - Deploy em containers com um único comando

### Exemplo de Uso

```bash
# Criar um pedido com desconto automático
POST /api/pedidos
{
  "idsItens": [1, 4, 5]  # X Burger + Batata Frita + Refrigerante
}

# Resposta
{
  "id": 1,
  "subtotal": 9.50,
  "percentualDesconto": 20,      # 20% automático!
  "valorDesconto": 1.90,
  "total": 7.60,                 # Total com desconto
  "itens": [...]
}
```

---

## 🛠️ Stack Tecnológico

### Backend
- **Linguagem**: C# 12.0
- **Framework**: ASP.NET Core 8.0
- **Runtime**: .NET 8.0

### Banco de Dados
- **Sistema**: PostgreSQL 16
- **ORM**: Entity Framework Core 8.0.5
- **Migrations**: EF Core Migrations (versionamento de schema)

### Bibliotecas Principais

| Biblioteca | Versão | Propósito |
|-----------|--------|----------|
| `Entity Framework Core` | 8.0.5 | ORM (mapeamento objeto-relacional) |
| `Npgsql.EntityFrameworkCore.PostgreSQL` | 8.0.4 | Driver PostgreSQL para EF Core |
| `FluentValidation` | 12.1.1 | Validação de dados fluida |
| `Swashbuckle.AspNetCore` | 6.5.0 | Documentação Swagger UI |
| `AutoMapper` | 16.1.1 | Mapeamento de objetos |
| `xUnit` | 2.4.2 | Framework de testes |
| `Moq` | 4.20.72 | Mocking para testes unitários |
| `FluentAssertions` | 8.9.0 | Assertions fluidas |

### Infraestrutura
- **Containerização**: Docker & Docker Compose
- **Serialização**: System.Text.Json

---

## 🏗️ Arquitetura

### Padrão: Clean Architecture + 3-Camadas

O projeto está organizado em **4 camadas independentes**, cada uma com responsabilidade clara:

```
┌─────────────────────────────────────────────────┐
│  GoodHamburger.API (Apresentação)              │
│  - Controllers                                  │
│  - Middlewares                                  │
│  - Configurações ASP.NET                        │
├─────────────────────────────────────────────────┤
│  GoodHamburger.Application (Aplicação)         │
│  - Services                                     │
│  - DTOs (Data Transfer Objects)                 │
│  - Validators (FluentValidation)                │
│  - Mapeadores                                   │
├─────────────────────────────────────────────────┤
│  GoodHamburger.Infrastructure (Acesso a Dados) │
│  - Repositories                                 │
│  - DbContext (EF Core)                          │
│  - Migrations                                   │
├─────────────────────────────────────────────────┤
│  GoodHamburger.Domain (Núcleo)                 │
│  - Entities                                     │
│  - Interfaces de Repository                     │
│  - Regras de Negócio Puras                      │
└─────────────────────────────────────────────────┘
```

### Domain Layer (Núcleo)
Contém **regras de negócio puras** em C#, sem dependências externas

### Application Layer (Orquestração)
Services que orquestram lógica, validação e usa Domain

### Infrastructure Layer (Dados)
Acesso a banco de dados via EF Core e Repositories

### API Layer (HTTP)
Controllers que expõem endpoints

---

## 💡 Decisões de Projeto

### 1. **Por que Clean Architecture?**

✅ **Separação de responsabilidades** - Cada camada tem um propósito
✅ **Testabilidade** - Domain não depende de banco/HTTP
✅ **Flexibilidade** - Trocar PostgreSQL por SQL Server é trivial
✅ **Escalabilidade** - Adicionar features não quebra código existente

### 2. **Por que Entity Framework Core?**

✅ **Migrations automáticas** - Versionamento de schema
✅ **LINQ type-safe** - Compilador verifica queries
✅ **Multi-banco** - Mesmo código funciona em PostgreSQL, SQL Server, MySQL
✅ **Relacionamentos automáticos** - EF cuida de JOINs

### 3. **Por que FluentValidation?**

✅ **Sintaxe fluida** - Mais legível que Data Annotations
✅ **Reutilizável** - Um validator em múltiplos contextos
✅ **Flexível** - Validações complexas (duplicatas, lógica customizada)

```csharp
public class CriarPedidoValidator : AbstractValidator<CriarPedidoRequestDto>
{
    public CriarPedidoValidator()
    {
        RuleFor(x => x.IdsItens)
            .NotEmpty()
            .Custom((ids, context) =>
            {
                if (ids.Count != ids.Distinct().Count())
                    context.AddFailure("Itens duplicados não permitidos");
            });
    }
}
```

### 4. **Por que DTOs (Data Transfer Objects)?**

✅ **Encapsulação** - Entity interna não é exposta
✅ **Validação** - DTO recebe só dados necessários
✅ **Versionamento** - API v1 com Entity diferente de v2

### 5. **Por que Async/Await em Tudo?**

✅ **Escalabilidade** - 1 thread atende múltiplos clientes
✅ **Responsividade** - Não bloqueia operações I/O
✅ **Performance** - Menos threads = menos memória

### 6. **Por que Docker & Docker Compose?**

✅ **Reprodutibilidade** - Dev/test/prod idênticos
✅ **Isolamento** - API e PostgreSQL separados mas conectados
✅ **Deploy trivial** - Um único comando (`docker-compose up`)

---

## 🚀 Como Rodar

### Pré-requisitos

- .NET 8.0 SDK ([Download](https://dotnet.microsoft.com/download))
- Docker & Docker Compose ([Download](https://www.docker.com/products/docker-desktop))
- Git

### Opção 1: Com Docker Compose (Recomendado)

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/goodhamburger.git
cd goodhamburger

# Inicie os containers (API + PostgreSQL)
docker-compose up

# Acesse
API:     http://localhost:5000
Swagger: http://localhost:5000/swagger
```

### Opção 2: Localmente (Desenvolvimento)

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/goodhamburger.git
cd goodhamburger

# Inicie apenas o PostgreSQL
docker-compose up postgres

# Restaure packages
dotnet restore

# Aplique migrations
dotnet ef database update --project src/GoodHamburger.Infrastructure

# Inicie a API
cd src/GoodHamburger.API
dotnet run

# Acesse
API:     http://localhost:5136
Swagger: http://localhost:5136/swagger
```

### Opção 3: Com Visual Studio

```
1. Abra GoodHamburger.sln
2. Clique com botão direito → Docker Compose → Set as Startup Project
3. Pressione F5
4. Swagger abre automaticamente
```

### Verificar se Funcionou

```bash
# Teste o endpoint de cardápio
curl http://localhost:5000/api/cardapio

# Resposta esperada
[
  {
    "id": 1,
    "nome": "X Burger",
    "preco": 5.00,
    "tipo": "Sanduiche",
    "ativo": true,
    "createdAt": "2026-04-19T23:24:03.15Z"
  }
  ...
]
```

---

## 📡 Endpoints

### Base URL
```
http://localhost:5000/api
```

### 🍔 Cardápio

#### Listar Todos os Itens
```http
GET /cardapio
```

**Resposta (200)**
```json
[
  {
    "id": 1,
    "nome": "X Burger",
    "preco": 5.00,
    "tipo": "Sanduiche",
    "createdAt": "2026-04-19T23:24:03.15Z",
    "updatedAt": null
  },
  {
    "id": 4,
    "nome": "Batata Frita",
    "preco": 2.00,
    "tipo": "Acompanhamento",
    "createdAt": "2026-04-19T23:24:03.15Z",
    "updatedAt": null
  },
  {
    "id": 5,
    "nome": "Refrigerante",
    "preco": 2.50,
    "tipo": "Bebida",
    "createdAt": "2026-04-19T23:24:03.15Z",
    "updatedAt": null
  }
]
```

---

### 🛒 Pedidos

#### Criar Novo Pedido
```http
POST /pedidos
Content-Type: application/json

{
  "idsItens": [1, 4, 5]
}
```

**Request**
```json
{
  "idsItens": [
    1,  // X Burger (R$ 5.00)
    4,  // Batata Frita (R$ 2.00)
    5   // Refrigerante (R$ 2.50)
  ]
}
```

**Resposta (201 Created)**
```json
{
  "id": 1,
  "itens": [
    {
      "menuItemId": 1,
      "nomeItem": "X Burger",
      "precoUnitario": 5.00,
      "tipo": "Sanduiche"
    },
    {
      "menuItemId": 4,
      "nomeItem": "Batata Frita",
      "precoUnitario": 2.00,
      "tipo": "Acompanhamento"
    },
    {
      "menuItemId": 5,
      "nomeItem": "Refrigerante",
      "precoUnitario": 2.50,
      "tipo": "Bebida"
    }
  ],
  "subtotal": 9.50,
  "percentualDesconto": 20,        // 🎉 Desconto automático!
  "valorDesconto": 1.90,
  "total": 7.60,                   // Total com desconto
  "createdAt": "2026-04-20T15:30:00Z",
  "updatedAt": null
}
```

**Erros**
```json
// 400 Bad Request - Itens duplicados
{
  "success": false,
  "errors": ["Não é permitido adicionar itens duplicados ao pedido."]
}

// 400 Bad Request - Item não existe
{
  "success": false,
  "errors": ["Item com ID 999 não encontrado."]
}

// 400 Bad Request - Múltiplos de mesmo tipo
{
  "success": false,
  "errors": ["O pedido deve conter no máximo 1 sanduíche, 1 acompanhamento e 1 bebida."]
}
```

---

#### Listar Todos os Pedidos
```http
GET /pedidos
```

**Resposta (200)**
```json
[
  {
    "id": 1,
    "itens": [...],
    "subtotal": 9.50,
    "percentualDesconto": 20,
    "valorDesconto": 1.90,
    "total": 7.60,
    "createdAt": "2026-04-20T15:30:00Z",
    "updatedAt": null
  }
]
```

---

#### Obter Pedido Específico
```http
GET /pedidos/{id}
```

**Exemplo**
```http
GET /pedidos/1
```

**Resposta (200)**
```json
{
  "id": 1,
  "itens": [...],
  "subtotal": 9.50,
  "percentualDesconto": 20,
  "valorDesconto": 1.90,
  "total": 7.60,
  "createdAt": "2026-04-20T15:30:00Z",
  "updatedAt": null
}
```

**Erros**
```json
// 404 Not Found
{
  "success": false,
  "errors": ["Pedido com ID 999 não encontrado."]
}
```

---

#### Atualizar Pedido
```http
PUT /pedidos/{id}
Content-Type: application/json

{
  "idsItens": [1, 2]
}
```

**Exemplo**
```http
PUT /pedidos/1
{
  "idsItens": [1, 2]
}
```

**Resposta (200)**
```json
{
  "id": 1,
  "itens": [
    {
      "menuItemId": 1,
      "nomeItem": "X Burger",
      "precoUnitario": 5.00,
      "tipo": "Sanduiche"
    },
    {
      "menuItemId": 2,
      "nomeItem": "X Egg",
      "precoUnitario": 4.50,
      "tipo": "Sanduiche"
    }
  ],
  "subtotal": 9.50,
  "percentualDesconto": 0,         // Sem desconto (2 sanduíches)
  "valorDesconto": 0.00,
  "total": 9.50,
  "createdAt": "2026-04-20T15:30:00Z",
  "updatedAt": "2026-04-20T15:35:00Z"
}
```

---

#### Deletar Pedido
```http
DELETE /pedidos/{id}
```

**Exemplo**
```http
DELETE /pedidos/1
```

**Resposta (200)**
```json
{
  "success": true,
  "message": "Pedido removido com sucesso",
  "data": null
}
```

**Erros**
```json
// 404 Not Found
{
  "success": false,
  "errors": ["Pedido com ID 999 não encontrado."]
}
```

---

## 📋 Regras de Negócio

### Desconto Inteligente

O sistema calcula automaticamente descontos baseado na **combinação de itens**:

| Combinação | Desconto | Exemplo |
|-----------|----------|---------|
| Sanduíche + Batata + Refrigerante | **20%** | X Burger (5.00) + Batata (2.00) + Refri (2.50) = 9.50 → **7.60** |
| Sanduíche + Refrigerante | **15%** | X Burger (5.00) + Refri (2.50) = 7.50 → **6.37** |
| Sanduíche + Batata | **10%** | X Burger (5.00) + Batata (2.00) = 7.00 → **6.30** |
| Outros (apenas sanduíche, etc) | **0%** | X Burger (5.00) = **5.00** |

**Exemplo de Cálculo**

```
Pedido: X Burger (5.00) + Batata Frita (2.00) + Refrigerante (2.50)

1. Subtotal = 5.00 + 2.00 + 2.50 = 9.50
2. Detecta combinação completa → Desconto = 20%
3. Valor Desconto = 9.50 * (20 / 100) = 1.90
4. Total = 9.50 - 1.90 = 7.60 ✅
```

### Validações Automáticas

| Validação | Erro | Exemplo |
|-----------|------|---------|
| Itens duplicados | "Não é permitido adicionar itens duplicados" | `[1, 1, 5]` ❌ |
| Item não existe | "Item com ID {id} não encontrado" | `[1, 999, 5]` ❌ |
| Múltiplos do mesmo tipo | "No máximo 1 sanduíche, 1 acompanhamento e 1 bebida" | `[1, 2, 3]` (3 sanduíches) ❌ |
| Pedido vazio | "O pedido deve conter pelo menos um item" | `[]` ❌ |

### Tipos de Itens

```
Sanduíches (IDs 1-3):
  - ID 1: X Burger (R$ 5.00)
  - ID 2: X Egg (R$ 4.50)
  - ID 3: X Bacon (R$ 7.00)

Acompanhamentos (ID 4):
  - ID 4: Batata Frita (R$ 2.00)

Bebidas (ID 5):
  - ID 5: Refrigerante (R$ 2.50)
```

---

## ✅ Testes

### Estrutura de Testes

```
tests/
├── GoodHamburger.Tests.Domain/          (17 testes unitários)
│   ├── MenuItemTests.cs                 (5 testes)
│   └── PedidoTests.cs                   (4 testes)
├── GoodHamburger.Tests.Application/     (5 testes unitários)
│   └── DescontoServiceTests.cs           (5 testes)
└── GoodHamburger.Tests.Integration/     (3 testes integração)
    └── PedidoServiceIntegrationTests.cs  (3 testes)
```

**Total: 25 testes automatizados**

### Executar Testes

```bash
# Todos os testes
dotnet test

# Teste específico
dotnet test --filter "MenuItemTests"

# Com saída verbosa
dotnet test --verbosity detailed
