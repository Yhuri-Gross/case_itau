# Clientes API

API REST desenvolvida em **.NET** para gerenciamento de clientes com operações bancárias básicas como cadastro, depósito, saque e exclusão.

O projeto foi desenvolvido seguindo boas práticas de arquitetura e inclui **testes unitários** para garantir a confiabilidade das regras de negócio.

---

# 🚀 Tecnologias utilizadas

* .NET
* ASP.NET Core Web API
* Entity Framework Core
* SQLite
* xUnit (Testes Unitários)
* Moq (Mocks para testes)
* Swagger

---

# 📁 Estrutura do projeto

```
Clientes.Api
│
├── Application
│   └── Services
│
├── Controllers
│
├── Domain
│   └── Entities
│
├── Infrastructure
│   ├── Data
│   └── Repositories
│
├── Middleware
│
└── Tests
    └── Clientes.Tests
```

---

# ⚙️ Funcionalidades

### Clientes

* Criar cliente
* Listar clientes
* Deletar cliente

### Operações financeiras

* Depositar valor
* Sacar valor

### Regras de negócio

* Email único por cliente
* Não permite saque maior que o saldo
* Saldo inicial do cliente é `0`

---

# 🔌 Endpoints

### Listar clientes

```
GET /clientes
```

---

### Criar cliente

```
POST /clientes
```

Body:

```json
{
  "nome": "João",
  "email": "joao@email.com"
}
```

---

### Deletar cliente

```
DELETE /clientes/{id}
```

---

### Depositar

```
POST /clientes/{id}/depositar
```

Body:

```json
100
```

---

### Sacar

```
POST /clientes/{id}/sacar
```

Body:

```json
50
```

---

# 🧪 Testes Unitários

O projeto inclui testes unitários para validar as regras de negócio da aplicação.

Framework utilizado:

* **xUnit**
* **Moq**

Para executar os testes:

```bash
dotnet test
```

---

# ▶️ Como rodar o projeto

Clone o repositório:

```bash
git clone https://github.com/Yhuri-Gross/case_itau
```

Entre na pasta do projeto:

```bash
cd Clientes.Api
```

Restaure os pacotes:

```bash
dotnet restore
```

Execute a aplicação:

```bash
dotnet run
```

A API estará disponível em:

```
http://localhost:5000
```

Swagger:

```
http://localhost:5000/swagger
```

---

# 🗄 Banco de dados

O projeto utiliza **SQLite** como banco de dados.

O banco é criado automaticamente na primeira execução da aplicação.

---

# 📌 Melhorias futuras

* Transferência entre clientes
* Autenticação com JWT
* Logs estruturados
* Paginação na listagem de clientes
* Dockerização da aplicação

---

