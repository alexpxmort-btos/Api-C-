
# ProductsApi

API de exemplo em **.NET 9** para gerenciamento de produtos.

---

## Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/) (opcional)
- [Visual Studio Code](https://code.visualstudio.com/) ou outra IDE de sua preferência

---

## Executando sem Docker

1. Abra o terminal na pasta do projeto:

```bash
cd Api/ProductsApi
````

2. Restaure as dependências do projeto:

```bash
dotnet restore
```

3. Execute a aplicação:

```bash
dotnet run
```

4. A API estará disponível em:

```
http://localhost:5199
```

5. Para acessar o Swagger (documentação interativa da API):

```
http://localhost:5199/swagger
```

---

## Executando com Docker

1. Certifique-se de que o Docker está instalado e rodando.

2. Na raiz do projeto (`Api/`), crie a imagem Docker:

```bash
docker build -t productsapi .
```

3. Execute o container:

```bash
docker run -d -p 5199:80 --name  productsapi
```

4. Agora a API estará disponível em:

```
http://localhost:5199
```

> **Observação:** o mapeamento de portas depende do que você definiu no `Dockerfile`. Ajuste se necessário.

---

## Testando a API

* Use o **Swagger** acessando `http://localhost:5199/swagger` ou
* Use o **Postman** ou qualquer cliente HTTP para testar os endpoints.

---

## Estrutura do Projeto

```
Api/
│
├─ ProductsApi/          -> Projeto principal da API
├─ ProductsApi.Tests/    -> Testes unitários
├─ dockerfile            -> Arquivo para criação da imagem Docker
├─ ProductsApi.sln       -> Solução do .NET
└─ README.md             -> Este arquivo
```

---

## Observações

* O projeto utiliza **InMemoryDatabase** por padrão, então os dados **não são persistidos** entre reinícios.
* Para produção, configure um banco de dados real no `appsettings.json` e registre o DbContext correspondente.


## FrontEnd
o front end esta no repositorio [Link](https://github.com/alexpxmort-btos/Products-X)