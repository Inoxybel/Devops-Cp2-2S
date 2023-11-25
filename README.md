# Documentação da API

## Visão Geral

Esta API permite a criação e o gerenciamento de contas de usuários. Ela suporta operações para criar um novo usuário, fazer login, atualizar usuário e senha, ativar conta e excluir um usuário.

## Endpoints

### Criar Novo Usuário

**POST** `/api/user`

#### Requisição:

```json
{
  "name": "Thiago",
  "email": "thiago@iacademy.tech",
  "cpf": "59577342094",
  "password": "@Senha123",
  "companyRef": ""
}
```

#### Respostas:

- **201 Criado** - Retorna o ID único do usuário criado.
- **400 Requisição Inválida** - A solicitação foi inválida.

### Logar

**POST** `/api/user/login`

#### Requisição:

```json
{
  "email": "thiago@iacademy.tech",
  "password": "@Senha123"
}
```

#### Respostas:

- **200 OK** - Autenticação foi bem-sucedida, retorna dados do usuário e token.
- **400 Requisição Inválida** - Credenciais inválidas ou campos faltantes.
- **404 Não Encontrado** - Usuário não encontrado.

### Atualizar Senha

**PUT** `/api/user/{id}/update-password`

#### Requisição:

```json
{
  "email": "thiago@iacademy.tech",
  "oldPassword": "@Senha123",
  "newPassword": "Novasenha1.",
  "confirmPassword": "Novasenha1."
}
```

#### Respostas:

- **204 Sem Conteúdo** - Senha atualizada com sucesso.
- **400 Requisição Inválida** - Campos inválidos ou senha antiga incorreta.
- **401 Não Autorizado** - Usuário não está autenticado.
- **404 Não Encontrado** - Usuário não encontrado.

### Ativar Usuário

**POST** `/api/user/{id}/active/{activationCode}`

#### Respostas:

- **200 OK** - Usuário ativado com sucesso.
- **400 Requisição Inválida** - Código de ativação inválido ou problemas na requisição.

### Excluir Usuário

**DELETE** `/api/user/{id}/{password}/delete`

#### Respostas:

- **204 Sem Conteúdo** - Usuário excluído com sucesso.
- **400 Requisição Inválida** - Problema na solicitação ou senha incorreta.
- **401 Não Autorizado** - Autenticação necessária.
- **404 Não Encontrado** - Usuário não encontrado.

## Evidências

- [Acesse esse link](/Evidencias/readme.md "Acesse esse link") para visualizar as evidências de criação da Infraestrutura

- [Acesse esse link](https://www.youtube.com/watch?v=SrSPFVgzq7I "Acesse esse link") para ver o vídeo da execução do CRUD da aplicação em nuvem Azure