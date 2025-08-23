# Sistema de Gerenciamento de Tarefas

Uma aplicação completa para gerenciamento de tarefas com backend em .NET 8 e frontend em Angular 19, incluindo autenticação JWT, dashboard com gráficos e paginação.

## 🚀 Tecnologias Utilizadas

### Backend
- **.NET 8.0** - Framework principal
- **ASP.NET Core Web API** - API REST
- **Entity Framework Core** - ORM
- **SQL Server** - Banco de dados
- **JWT Bearer Authentication** - Autenticação
- **Swagger/OpenAPI** - Documentação da API

### Frontend
- **Angular 19** - Framework frontend
- **TypeScript** - Linguagem principal
- **Tailwind CSS** - Framework CSS
- **Chart.js** - Gráficos e visualizações
- **Angular Reactive Forms** - Formulários reativos
- **JWT Authentication** - Autenticação integrada

### DevOps
- **Docker & Docker Compose** - Containerização
- **Hot Reload** - Desenvolvimento em tempo real

## 📋 Pré-requisitos

### Opção 1: Docker (Recomendado)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

### Opção 2: Desenvolvimento Local
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/) e [npm](https://www.npmjs.com/)
- [Angular CLI 19+](https://angular.io/cli)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) ou [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-editions-express)

## 🔧 Como Executar

### Usando Docker Compose (Recomendado)

1. **Clone o repositório:**
   ```bash
   git clone <url-do-repositorio>
   cd GerenciamentoDeTarefas
   ```

2. **Execute com Docker Compose:**
   ```bash
   docker compose up -d
   ```

3. **Acesse a aplicação:**
   - **Frontend**: http://localhost:4201
   - **API**: http://localhost:5000
   - **Swagger UI**: http://localhost:5001/swagger
   - **Banco de dados**: localhost:1433

### Desenvolvimento Local

#### Backend

1. **Navegue para a pasta do backend:**
   ```bash
   cd backend
   ```

2. **Configure a string de conexão:**
   Edite o arquivo `appsettings.Development.json` com sua string de conexão do SQL Server:
   ```json
   {
     "ConnectionStrings": {
       "Default": "Server=localhost;Database=GerenciamentoTarefas;Trusted_Connection=true;TrustServerCertificate=true;"
     }
   }
   ```

3. **Execute as migrações do banco:**
   ```bash
   dotnet ef database update
   ```

4. **Execute a aplicação:**
   ```bash
   dotnet run
   ```

#### Frontend

1. **Navegue para a pasta do frontend:**
   ```bash
   cd frontend
   ```

2. **Instale as dependências:**
   ```bash
   npm install
   ```

3. **Execute o servidor de desenvolvimento:**
   ```bash
   npm start
   ```

4. **Acesse as aplicações:**
   - **Frontend**: http://localhost:4200
   - **API**: http://localhost:5134
   - **Swagger UI**: http://localhost:5134/swagger

## ✨ Funcionalidades

### 🎯 Sistema de Autenticação
- Login e logout de usuários
- Autenticação JWT com refresh token
- Controle de acesso baseado em roles (Admin/User)
- Interceptor automático para requisições autenticadas

### 👥 Gerenciamento de Usuários
- Listagem de usuários (Admin apenas)
- Criação de novos usuários
- Edição de informações do usuário
- Diferentes níveis de acesso

### 📋 Gerenciamento de Tarefas
- **CRUD completo** de tarefas
- **Filtros avançados** por status e responsável
- **Paginação** com controle de itens por página
- **Status das tarefas**: Pendente, Em andamento, Concluída
- **Atribuição** de responsáveis às tarefas
- **Permissões**: Usuários normais só gerenciam suas próprias tarefas

### 📊 Dashboard Interativo
- **Gráficos em tempo real** com Chart.js
- **Estatísticas** de tarefas por status
- **Distribuição** de tarefas por responsável
- **Métricas** de produtividade

### 🎨 Interface Moderna
- **Design responsivo** com Tailwind CSS
- **Componentes reutilizáveis** em Angular
- **Formulários reativos** com validação
- **Feedback visual** de carregamento e erros

## 🔐 Autenticação

A API utiliza JWT Bearer Authentication. Para acessar os endpoints protegidos:

1. **Faça login** através do endpoint `/api/Auth/login` com credenciais válidas
2. **Use o token JWT** retornado no header `Authorization: Bearer <token>`
3. **No Swagger UI**, clique no botão "Authorize" e insira o token

### Usuários Padrão (Seed Data)

O sistema cria automaticamente os seguintes usuários para teste:

- **Admin:**
  - Email: `admin@taskmanager.com`
  - Senha: `Admin123!`
  - Role: Admin

- **Usuário:**
  - Email: `user@taskmanager.com`
  - Senha: `User123!`
  - Role: User

## 📚 Endpoints Principais

### Autenticação
- `POST /api/Auth/login` - Login do usuário
- `POST /api/Auth/refresh` - Renovar token JWT

### Usuários
- `GET /api/Users` - Listar usuários (Admin apenas)
- `POST /api/Users` - Criar usuário
- `PUT /api/Users/{id}` - Editar usuário

### Tarefas
- `GET /api/Tasks` - Listar tarefas com filtros e paginação
- `GET /api/Tasks/{id}` - Obter tarefa específica
- `POST /api/Tasks` - Criar tarefa
- `PUT /api/Tasks/{id}` - Editar tarefa
- `DELETE /api/Tasks/{id}` - Deletar tarefa

### Dashboard
- `GET /api/Tasks/dashboard` - Dashboard com estatísticas e métricas

## 🗂 Estrutura do Projeto

```
GerenciamentoDeTarefas/
├── backend/                    # API .NET 8
│   ├── src/
│   │   ├── Api/
│   │   │   ├── Controllers/    # Controllers da API
│   │   │   └── Middlewares/    # Middlewares customizados
│   │   ├── Application/
│   │   │   ├── DTOs/          # Data Transfer Objects
│   │   │   ├── Interfaces/    # Interfaces de serviços
│   │   │   ├── Services/      # Serviços de aplicação
│   │   │   └── UseCases/      # Casos de uso
│   │   ├── Domain/
│   │   │   ├── Entities/      # Entidades do domínio
│   │   │   └── Interfaces/    # Interfaces do domínio
│   │   └── Infra/
│   │       ├── Data/          # Contexto do EF Core
│   │       └── Repositories/  # Implementação dos repositórios
│   ├── Migrations/            # Migrações do Entity Framework
│   ├── Properties/            # Configurações de launch
│   ├── Dockerfile             # Dockerfile do backend
│   └── Program.cs             # Ponto de entrada da aplicação
├── frontend/                  # Aplicação Angular 19
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/    # Componentes da aplicação
│   │   │   │   ├── dashboard/ # Dashboard com gráficos
│   │   │   │   ├── login/     # Página de login
│   │   │   │   ├── tasks/     # Gerenciamento de tarefas
│   │   │   │   └── users/     # Gerenciamento de usuários
│   │   │   ├── guards/        # Guards de autenticação
│   │   │   ├── interceptors/  # Interceptors HTTP
│   │   │   ├── models/        # Modelos TypeScript
│   │   │   └── services/      # Serviços Angular
│   │   ├── styles.css         # Estilos globais com Tailwind
│   │   └── index.html
│   ├── Dockerfile             # Dockerfile do frontend
│   ├── package.json           # Dependências Node.js
│   ├── tailwind.config.js     # Configuração do Tailwind
│   └── angular.json           # Configuração do Angular
├── docker-compose.yml         # Orquestração dos containers
├── GerenciamentoDeTarefas.sln # Solution .NET
└── README.md                  # Documentação do projeto
```

## 🐳 Docker

### Comandos Úteis

```bash
# Subir os serviços
docker compose up -d

# Ver logs
docker compose logs -f

# Parar os serviços
docker compose down

# Rebuild das imagens
docker compose build --no-cache

# Remover volumes (limpar banco de dados)
docker compose down -v
```

### Portas dos Serviços

- **Frontend Angular**: 4201
- **API .NET**: 5001
- **SQL Server**: 1433
- **Swagger UI**: http://localhost:5001/swagger

## 🔧 Desenvolvimento

### Backend (.NET 8)

#### Executar Migrações
```bash
cd backend

# Adicionar nova migração
dotnet ef migrations add NomeDaMigracao

# Aplicar migrações
dotnet ef database update

# Remover última migração
dotnet ef migrations remove
```

#### Build e Execução
```bash
cd backend

# Build do projeto
dotnet build

# Executar a aplicação
dotnet run

# Executar com watch (hot reload)
dotnet watch run
```

### Frontend (Angular 19)

#### Comandos Angular
```bash
cd frontend

# Instalar dependências
npm install

# Servidor de desenvolvimento
npm start
# ou
ng serve

# Build para produção
npm run build
# ou
ng build

# Executar testes
npm test
# ou
ng test

# Gerar componente
ng generate component nome-do-componente

# Gerar serviço
ng generate service nome-do-servico
```

#### Estrutura de Componentes
- **Dashboard**: Gráficos e estatísticas com Chart.js
- **Tasks**: CRUD de tarefas com paginação e filtros
- **Users**: Gerenciamento de usuários (Admin apenas)
- **Login**: Autenticação de usuários

#### Guards e Interceptors
- **AuthGuard**: Protege rotas que requerem autenticação
- **AdminGuard**: Protege rotas exclusivas para administradores
- **AuthInterceptor**: Adiciona automaticamente tokens JWT nas requisições

## 📝 Notas Importantes

### Configuração e Deploy
- O banco de dados é criado automaticamente na primeira execução
- Os dados de seed são inseridos automaticamente
- Para desenvolvimento, use as credenciais de usuário fornecidas acima
- O Swagger UI está disponível apenas em ambiente de Development

### Frontend
- A aplicação Angular utiliza **Tailwind CSS** para estilização
- **Chart.js** é usado para renderização de gráficos no dashboard
- O sistema de **paginação** funciona integrado com os filtros
- **Interceptors** gerenciam automaticamente a autenticação JWT
- **Guards** protegem rotas baseado no tipo de usuário

### Backend
- API RESTful seguindo padrões de **Clean Architecture**
- **JWT Authentication** com refresh token automático
- **Entity Framework Core** com migrações automáticas
- **Swagger/OpenAPI** para documentação interativa

### Docker
- **Hot reload** habilitado para desenvolvimento
- **Multi-stage builds** para otimização de imagem
- **Volumes persistentes** para banco de dados
- **Networks isoladas** para comunicação entre serviços
