# Correios Automation Tests

Projeto de automação de testes para o site dos Correios utilizando C#, SpecFlow, NUnit e Selenium WebDriver.

---

## Sobre o projeto

Esse projeto foi criado com o objetivo de validar, de forma automatizada, algumas funcionalidades básicas do site dos Correios, como busca de CEP e rastreamento de objetos.

A ideia aqui foi aplicar boas práticas de automação, como uso de Page Object, organização por camadas e cenários em BDD com SpecFlow.

---

## O que está sendo testado

Os cenários cobertos são:

* Busca de um CEP inválido (80700000) e validação da mensagem de retorno
* Busca de um CEP válido (01013-001) e verificação do endereço retornado
* Tentativa de rastreamento com código inválido (SS987654321BR)
* Validação de elementos utilizando diferentes tipos de seletores (ID, XPath e CSS)

---

## Tecnologias utilizadas

* .NET 6
* SpecFlow (BDD com Gherkin)
* NUnit
* Selenium WebDriver
* ChromeDriver

---

## Pré-requisitos

Para rodar o projeto, você vai precisar ter instalado:

* .NET 6 SDK
* Visual Studio 2022 (ou VS Code)
* Google Chrome atualizado
* Git

Extensões recomendadas no Visual Studio:

* SpecFlow for Visual Studio
* NUnit Test Adapter

---

## Como rodar o projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/ThiagoPereira80/correios-automation-tests.git
cd correios-automation-tests
```

### 2. Restaurar dependências

```bash
dotnet restore
```

### 3. Executar os testes

Você pode rodar de duas formas:

* Pelo Visual Studio (Test Explorer)
* Ou via terminal:

```bash
dotnet test
```

---

## Estrutura do projeto

O projeto está organizado da seguinte forma:

```
Features   -> Cenários em Gherkin
Steps      -> Implementação dos passos
Pages      -> Page Objects (interação com a UI)
Hooks      -> Setup e teardown do WebDriver
```

---

## Seletores utilizados

Para atender o requisito do teste, foram utilizados três tipos de seletores:

* ID (mais performático quando disponível)
* CSS Selector
* XPath

---

## Otimizações aplicadas

Alguns cuidados foram tomados para melhorar performance e manutenção:

* Execução em modo headless (mais rápido)
* Uso de WebDriverWait (evitando Thread.Sleep)
* Separação por Page Object
* Reaproveitamento do driver via Hooks

---

## Execução em pipeline / servidor

O projeto pode ser facilmente integrado com ferramentas de CI como Jenkins, Azure DevOps ou GitHub Actions, permitindo execução automática dos testes.

---

## Versionamento (Git)

Fluxo básico utilizado:

```bash
git add .
git commit -m "feat: automação correios"
git push
```

---

## Resultado esperado

Ao executar os testes, todos os cenários devem passar, validando:

* Mensagem de CEP não encontrado
* Retorno correto para CEP válido
* Mensagem de erro para rastreamento inválido

---

## Observações

O site dos Correios pode sofrer alterações com frequência, então os seletores podem precisar de ajustes ao longo do tempo.

---

## Autor

Projeto desenvolvido como parte de avaliação técnica para vaga de QA.
