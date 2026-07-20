# Diário de Desenvolvimento

## Sessão 1 - Análise inicial do desafio

### Objetivo

Entender completamente o problema antes de iniciar a implementação.

### Resultado

Foi realizada a análise inicial do enunciado, identificando requisitos, diferenciais, funcionalidades opcionais e as principais ambiguidades que precisarão ser resolvidas antes da implementação.

### Atividades realizadas

- Leitura e interpretação do enunciado do desafio.
- Separação entre regras obrigatórias, diferenciais e funcionalidades opcionais.
- Identificação do objetivo principal do sistema e das informações de entrada e saída.
- Levantamento das principais ambiguidades presentes no enunciado.
- Criação do documento `assumptions.md` para registrar questões em aberto e futuras decisões de projeto.

### Principais conclusões

- Existem regras obrigatórias.
- Existem diferenciais.
- Existem funcionalidades opcionais.
- Foram identificadas as principais decisões necessárias para definir o comportamento do MVP (Minimun Viable Product).
- Ainda existem ambiguidades importantes que precisam ser resolvidas antes da modelagem.

### Próximo passo

Analisar as ambiguidades do enunciado, avaliar as alternativas para cada uma delas e registrar as primeiras premissas do projeto.

---

## Sessão 2 - Definição das premissas do MVP

### Objetivo

Analisar as principais ambiguidades do enunciado e definir as primeiras premissas do projeto para orientar o desenvolvimento do MVP.

### Resultado

Foram avaliadas diferentes alternativas para os principais pontos não especificados no enunciado. Parte das ambiguidades foi resolvida e transformada em premissas do projeto, enquanto outras permaneceram em análise.

### Atividades realizadas

- Análise do significado de uma viagem.
- Avaliação das alternativas para definição da base dos drones.
- Avaliação da necessidade de retorno à base.
- Comparação entre fórmulas para cálculo da distância.
- Definição do cálculo da distância total de uma viagem.
- Avaliação das alternativas para utilização da prioridade dos pedidos.
- Definição do nível de otimização esperado para o MVP.
- Avaliação da reutilização dos drones durante a simulação.
- Definição da estratégia para tratamento de pedidos impossíveis.
- Separação entre interpretações do enunciado e decisões de projeto.

### Principais conclusões

- Uma viagem poderá agrupar vários pedidos e visitar vários destinos.
- Todos os drones partirão de uma base única localizada em `(0,0)`.
- Toda viagem considerará o retorno do drone à base.
- Será utilizada distância euclidiana para calcular o deslocamento entre coordenadas.
- A distância total incluirá todos os trechos percorridos durante a viagem.
- Os pedidos serão inicialmente considerados em ordem de prioridade, de Alta para Média e Baixa.
- Pedidos com prioridades diferentes poderão ser agrupados na mesma viagem.
- O MVP utilizará uma heurística simples para reduzir o número de viagens.
- Um mesmo drone poderá realizar várias viagens.
- Pedidos impossíveis serão tratados separadamente.
- Permaneceu em análise a estratégia de ordenação dos destinos dentro de uma viagem.

### Próximo passo

Definir a estratégia de ordenação dos destinos dentro de uma viagem e os critérios de desempate antes de iniciar a modelagem da solução.

---

## Sessão 3 — Definição da estratégia de planejamento

### Objetivo

Definir a heurística utilizada para formar viagens, selecionar pedidos adicionais, organizar os destinos e validar o alcance dos drones.

### Resultado

Foi definida a estratégia principal de formação das viagens, incluindo a escolha inicial por prioridade e peso, a avaliação de candidatos, o recálculo da rota, os critérios de desempate durante o agrupamento e o roteamento, a validação das restrições e as condições de encerramento.

Permaneceram pendentes a política de seleção dos drones e o desempate completo para escolha do primeiro pedido de cada viagem.

### Atividades realizadas

- Comparação de alternativas para escolha do primeiro pedido da viagem
- Análise de critérios para agrupamento de pedidos
- Definição da heurística do vizinho mais próximo para organização da rota
- Análise da relação entre seleção de candidatos e recálculo da rota
- Comparação entre avaliação pela ordem atual, melhor inserção e recálculo completo da heurística
- Definição das condições de encerramento de uma viagem
- Identificação das limitações da estratégia escolhida.

### Decisões tomadas

- Os pedidos são inicialmente considerados por prioridade
- Uma viagem começa pelo pedido mais pesado dentro da maior prioridade disponível
- Cada pedido candidato é incluído temporariamente para avaliação
- A rota temporária é recalculada utilizando o vizinho mais próximo
- O cálculo da distância inclui o retorno à base
- Candidatos que ultrapassarem peso ou alcance são desconsiderados para a viagem atual, mas permanecem pendentes para avaliações posteriores.
- Entre os candidatos válidos, é escolhido o que produzir o menor aumento de distância
- Os desempates entre candidatos seguem maior prioridade, maior peso, menor ordem de entrada e menor identificador.
- A rota simulada do candidato escolhido torna-se a rota oficial da viagem
- A viagem termina quando nenhum pedido adicional puder ser incluído validamente.

### Limitações reconhecidas

- O vizinho mais próximo não garante a menor rota
- A escolha local do menor aumento não garante o menor número global de viagens
- A heurística poderá executar vários recálculos de rota
- O custo adicional é considerado aceitável para o volume esperado no MVP.

### Decisão complementar

Foi definido que empates durante a execução da heurística do vizinho mais próximo serão resolvidos pela seguinte ordem:

1. Maior prioridade
2. Menor ordem de entrada
3. Maior peso
4. Menor identificador.

A prioridade preserva a urgência dos pedidos, enquanto a ordem de entrada favorece previsibilidade e evita ultrapassagens desnecessárias.

O peso foi mantido como terceiro critério para preservar a consistência com a estratégia geral de tratar pacotes mais pesados mais cedo. O identificador será utilizado apenas para garantir determinismo em caso de empate completo.

### Próximo passo

Descrever o algoritmo completo de planejamento em linguagem natural. Em seguida, validar seu comportamento por meio de cenários pequenos antes de iniciar a modelagem técnica.

---

## Sessão 4 — Consolidação da especificação funcional

### Objetivo

Consolidar as decisões da estratégia de planejamento em uma especificação funcional completa, eliminando as ambiguidades restantes antes da modelagem da solução.

### Resultado

A especificação funcional do algoritmo foi consolidada.

Foram definidas as últimas regras que permaneciam em aberto, tornando o comportamento do planejamento determinístico e consolidando uma especificação funcional suficiente para implementação do MVP.

### Atividades realizadas

- Revisão da especificação funcional do algoritmo
- Identificação das lacunas restantes
- Comparação de alternativas para seleção dos drones
- Definição da política de seleção e reutilização dos drones
- Comparação de alternativas para o desempate na escolha do primeiro pedido
- Consolidação das decisões finais da estratégia de planejamento

### Decisão — Seleção e reutilização dos drones

Antes de cada nova viagem, serão identificados os drones capazes de atender individualmente pelo menos um pedido ainda pendente, considerando simultaneamente capacidade de peso e alcance necessário para sair da base, realizar a entrega e retornar à base.

Entre os drones elegíveis, será escolhido aquele com maior capacidade de peso.

Em caso de empate, serão utilizados, nesta ordem:

1. Maior alcance
2. Menor ordem de entrada
3. Menor identificador, quando aplicável

A seleção será repetida antes de cada nova viagem.

Todos os drones elegíveis voltarão a participar da seleção, permitindo reutilização ilimitada.

Não haverá rodízio, balanceamento, reserva ou preferência por drones ainda não utilizados.

### Justificativa

A política prioriza drones com maior potencial de agrupamento de pedidos, mantendo um comportamento simples, determinístico e coerente com o objetivo do MVP.

Por se tratar de uma heurística, não existe garantia de minimizar globalmente o número de viagens.

A escolha privilegia simplicidade e previsibilidade em detrimento de uma busca exaustiva pela solução ótima.

### Decisão — Desempate na escolha do primeiro pedido

Entre os pedidos atendíveis pelo drone selecionado, cada nova viagem será iniciada pelo pedido de maior peso dentro da maior prioridade disponível.

Quando dois ou mais pedidos possuírem a mesma prioridade e o mesmo peso, será escolhido aquele com menor ordem de entrada.

Persistindo o empate, será utilizado o menor identificador.

A ordem de entrada será preservada de forma estável e única durante todo o planejamento.

### Justificativa

A utilização da menor ordem de entrada mantém o comportamento previsível entre pedidos equivalentes, preserva a sequência de recebimento e evita introduzir uma nova heurística geográfica antes da formação da rota.

A decisão também mantém consistência com os demais critérios de desempate já utilizados pelo algoritmo.

### Decisão — Critérios de desempate em diferentes etapas do algoritmo

Durante a consolidação da especificação funcional foi avaliada a possibilidade de utilizar exatamente a mesma ordem de desempate em todos os pontos do algoritmo.

Optou-se por manter critérios diferentes entre a formação da viagem e a heurística do vizinho mais próximo.

Na formação da viagem, o peso permanece antes da ordem de entrada, pois pedidos mais pesados tendem a ser mais difíceis de acomodar dentro das restrições do drone.

Na ordenação dos destinos, todos os pedidos já pertencem à viagem.

Nessa etapa, a ordem de entrada passa a ter maior relevância por preservar previsibilidade entre destinos logisticamente equivalentes, enquanto o peso permanece apenas como critério secundário.

A diferença foi considerada intencional e passou a fazer parte da especificação funcional do algoritmo.

Essa decisão evita aplicar um mesmo critério em contextos cuja finalidade é diferente, preservando a coerência da heurística em cada etapa do planejamento.

### Limitações reconhecidas

- A política de seleção dos drones é heurística e não garante o menor número global de viagens
- A ordem de entrada não representa eficiência logística
- Alterações na sequência original dos pedidos podem modificar a composição das viagens, mesmo mantendo os mesmos dados de entrada
- Essas limitações são consideradas aceitáveis para o escopo do MVP, que prioriza simplicidade, previsibilidade e facilidade de validação
- A estratégia heurística não garante a melhor rota possível para cada viagem.

### Principais conclusões

- Todas as regras funcionais necessárias para o planejamento das viagens foram definidas
- Os critérios de desempate foram revisados individualmente conforme a finalidade de cada etapa do algoritmo
- O algoritmo possui comportamento determinístico para todos os cenários previstos no MVP
- As limitações conhecidas foram documentadas e aceitas como parte da estratégia heurística adotada

### Próximo passo

Validar a especificação funcional por meio de cenários representativos e, após sua aprovação, iniciar a modelagem técnica e a implementação.

---

## Sessão 5 — Modelagem conceitual do domínio

### Objetivo

Definir a estrutura conceitual do domínio antes da implementação, identificando os principais objetos, suas responsabilidades e seus relacionamentos, mantendo uma arquitetura simples e adequada ao escopo do MVP.

### Resultado

Foi concluída a modelagem conceitual do domínio, consolidada no documento `domain-model.md` e em seu respectivo diagrama de classes conceitual.

A modelagem foi refinada para representar apenas os conceitos essenciais do problema, separando claramente objetos do domínio, resultados do planejamento e componentes responsáveis pelo comportamento do algoritmo.

### Atividades realizadas

- Identificação dos principais objetos do domínio.
- Definição das responsabilidades de cada objeto.
- Definição dos relacionamentos entre os objetos.
- Avaliação da separação entre dados do domínio e componentes de comportamento.
- Revisão da modelagem para reduzir complexidade desnecessária.
- Comparação entre manter ou remover o objeto `Route`.
- Definição da representação dos pedidos impossíveis.
- Definição da prioridade como conjunto fechado de valores.
- Elaboração da documentação `domain-model.md`.
- Elaboração do diagrama de classes conceitual.

### Decisões tomadas

- O domínio será representado pelos objetos `Drone`, `Order`, `Coordinate`, `Trip`, `PlanningResult` e `ImpossibleOrder`.
- O objeto `Route` foi removido da modelagem por duplicar informações já representadas em `Trip`.
- A ordem da rota será representada pela sequência dos pedidos armazenados em cada viagem.
- A prioridade será representada por um conjunto fechado de valores (`High`, `Medium` e `Low`).
- Os pedidos impossíveis serão representados juntamente com o motivo da impossibilidade.
- O componente `TripPlanner` coordenará o fluxo do algoritmo.
- O cálculo de distância permanecerá isolado em `DistanceCalculator`.
- O recálculo da rota utilizando a heurística do Vizinho Mais Próximo será responsabilidade de `NearestNeighborRouteCalculator`.

### Limitações reconhecidas

- A modelagem representa apenas o domínio do problema, sem considerar persistência, API ou infraestrutura.
- Não foram introduzidas abstrações voltadas para extensões futuras que não façam parte do escopo do MVP.
- A distribuição definitiva das responsabilidades poderá sofrer pequenos ajustes durante a implementação, desde que preserve a estrutura conceitual definida.

### Principais conclusões

- A modelagem cobre todos os conceitos necessários para implementar a especificação funcional.
- As responsabilidades dos objetos foram definidas de forma clara e com baixo acoplamento.
- O domínio foi mantido independente de detalhes de infraestrutura.
- A documentação conceitual servirá como referência para a implementação das classes em C#.

### Lições aprendidas

A modelagem conceitual mostrou que representar corretamente o domínio é diferente de projetar a implementação. Antes de definir métodos, padrões ou detalhes técnicos, foi necessário identificar quais conceitos realmente pertencem ao problema de negócio e quais responsabilidades cada objeto deve assumir.

### Próximo passo

Projetar o modelo de implementação, definindo como os conceitos do domínio serão representados em C#, suas estruturas, tipos, encapsulamento e componentes de comportamento antes do início da implementação.

---

---

## Sessão 6 — Planejamento técnico da solução

### Objetivo

Definir a organização da solução e projetar como os conceitos do domínio serão representados em C#, antes do início da implementação.

### Resultado

Foi definida uma arquitetura simples para o MVP, composta por dois projetos de produção e dois projetos de testes.

Também foi concluído o projeto dos principais modelos de implementação, incluindo seus tipos, atributos, responsabilidades, encapsulamento, mutabilidade e regras de validação.

### Atividades realizadas

- Definição dos projetos que compõem a solution.
- Definição das responsabilidades e dependências entre os projetos.
- Definição da localização dos testes unitários e de integração.
- Avaliação da necessidade de uma camada de aplicação.
- Simplificação da arquitetura para evitar camadas sem responsabilidade concreta.
- Definição da representação dos modelos do domínio em C#.
- Comparação entre `class`, `record` e `record struct`.
- Definição da estratégia de imutabilidade dos modelos.
- Definição da representação da ordem de entrada de drones e pedidos.
- Avaliação da representação dos motivos de impossibilidade.
- Separação das responsabilidades de validação entre API, modelos do domínio e `TripPlanner`.
- Definição da exposição somente para leitura das coleções de resultados.

### Decisão — Estrutura da solution

A solution será organizada nos seguintes projetos:

```text
DroneDeliveryCase.sln

src/DroneDelivery.Domain + /DroneDelivery.Api/

tests/DroneDelivery.Domain.Tests/ + /DroneDelivery.Api.IntegrationTests/

```

O projeto `DroneDelivery.Domain` concentrará os modelos do domínio, o algoritmo de planejamento e os componentes auxiliares de cálculo.

O projeto `DroneDelivery.Api` será responsável pelos contratos HTTP, conversão entre contratos e domínio, configuração da aplicação e exposição do endpoint.

O projeto `DroneDelivery.Domain.Tests` conterá os testes unitários das regras do domínio e do algoritmo.

O projeto `DroneDelivery.Api.IntegrationTests` conterá os testes de integração do endpoint e do fluxo HTTP.

Não será criado um projeto `Application`, pois o MVP não possui casos de uso ou orquestrações adicionais que justifiquem uma camada intermediária. A API chamará diretamente o `TripPlanner`.


### Decisão — Representação dos modelos em C#

Os modelos serão representados da seguinte forma:

| Modelo | Representação |
|--------|---------------|
| `Drone` | `class` |
| `Order` | `class` |
| `Coordinate` | `readonly record struct` |
| `Trip` | `class` |
| `ImpossibleOrder` | `class` |
| `PlanningResult` | `class` |

`Coordinate` será representado como um tipo por valor porque não possui identidade própria, deve ser imutável e duas coordenadas com os mesmos valores representam a mesma posição.

Os demais modelos serão implementados como classes. Não será utilizado `sealed`, pois o domínio não possui hierarquias e restringir a herança não acrescentaria benefício relevante ao MVP.


### Decisão — Imutabilidade e encapsulamento

Os modelos serão criados em estados válidos e não serão alterados durante a execução do planejamento.

Não serão utilizados setters públicos para propriedades que fazem parte do estado dos objetos.

O `TripPlanner` poderá utilizar coleções e variáveis internas mutáveis durante a execução, mas os objetos de entrada e os resultados finais permanecerão estáveis.

As coleções de `Trip` e `PlanningResult` serão expostas somente para leitura e protegidas contra alterações externas.


### Decisão — Ordem de entrada

`Drone` e `Order` possuirão a propriedade `InputOrder`.

Esse valor é necessário para os critérios determinísticos de desempate definidos na especificação funcional.

O cliente não enviará `InputOrder` no contrato HTTP. A API atribuirá o valor com base na posição de cada elemento nas coleções de drones e pedidos recebidas na requisição.

Dessa forma, a ordem da coleção será a única fonte de verdade, enquanto o domínio preservará o valor durante toda a execução do algoritmo.


### Decisão — Motivo dos pedidos impossíveis

O motivo associado a um `ImpossibleOrder` será representado pelo enum `ImpossibleReason`, em vez de uma string livre.

A utilização de um conjunto fechado de valores evita inconsistências textuais, facilita testes e mantém as causas de impossibilidade explicitamente representadas no domínio.

A API poderá converter o valor para uma representação textual apropriada na resposta HTTP.


### Decisão — Validação

As validações serão distribuídas conforme a responsabilidade de cada componente.

A API será responsável por:

- validar a estrutura da requisição
- identificar campos obrigatórios ausentes
- validar a conversão dos valores recebidos
- converter os contratos HTTP em objetos do domínio.

Os construtores dos modelos serão responsáveis por impedir estados intrinsecamente inválidos, como:

- identificadores vazios
- pesos, capacidades e alcances inválidos
- coordenadas não finitas
- coleções nulas
- objetos de resultado incompletos.

O `TripPlanner` será responsável pelas validações que dependem do conjunto completo de drones e pedidos ou da execução do algoritmo, como:

- identificadores duplicados
- definição de pedidos impossíveis
- seleção de drones elegíveis
- respeito aos limites de capacidade e alcance
- garantia de que cada pedido apareça uma única vez no resultado
- consistência final do planejamento.

Serão utilizadas apenas exceções padrão de argumento do .NET para impedir a criação de objetos inválidos.

Pedidos impossíveis não serão tratados como exceções, pois representam um resultado esperado do planejamento.

### Principais conclusões

- A arquitetura da solution foi mantida simples e adequada ao escopo do MVP.
- O domínio permanecerá independente da API e de detalhes de infraestrutura.
- Não será criada uma camada de aplicação sem responsabilidade concreta.
- Os modelos serão predominantemente imutáveis.
- `Coordinate` terá semântica de valor.
- A ordem de entrada será preservada no domínio, mas derivada das coleções recebidas pela API.
- Os motivos de impossibilidade serão representados por um enum.
- As validações foram distribuídas entre API, construtores e `TripPlanner` conforme o conhecimento de cada componente.
- As decisões são suficientes para orientar a implementação sem alterar o modelo conceitual aprovado.

### Lições aprendidas

O planejamento técnico demonstrou que uma arquitetura com mais camadas não é necessariamente uma arquitetura melhor.

A estrutura da solução deve refletir responsabilidades reais do sistema. Para este MVP, manter o domínio independente e permitir que a API chame diretamente o planejador oferece organização suficiente sem introduzir abstrações que não agregariam valor.

Também foi possível distinguir a modelagem conceitual do projeto de implementação: o primeiro define os conceitos existentes no domínio, enquanto o segundo define como esses conceitos serão representados tecnicamente em C#.

### Próximo passo

Definir os enums utilizados pelo domínio e planejar os casos de teste que validarão as regras do algoritmo antes do início da implementação.

---

## Sessão 7 — Definição dos enums e planejamento dos testes

### Objetivo

Consolidar os últimos elementos do domínio antes da implementação e definir a estratégia de testes do MVP.

### Resultado

Foram definidos os enums utilizados pelo domínio e estabelecida a estratégia geral da suíte de testes, incluindo sua organização, prioridades e principais cenários.

### Atividades realizadas

- Definição dos enums do domínio.
- Avaliação da necessidade de valores neutros.
- Definição da precedência explícita das prioridades.
- Planejamento da suíte de testes.
- Separação entre testes unitários e de integração.
- Priorização dos cenários críticos.

### Decisões tomadas

- O domínio utilizará apenas os enums `Priority` e `ImpossibleReason`.
- Nenhum dos enums possuirá valores neutros.
- A precedência `High > Medium > Low` será aplicada explicitamente pela lógica do algoritmo.
- A maior parte da cobertura ficará em `DroneDelivery.Domain.Tests`.
- Os testes de integração ficarão concentrados em `DroneDelivery.Api.IntegrationTests`.
- Os testes serão implementados por prioridade, iniciando pelas regras centrais do algoritmo.

### Principais conclusões

- Todos os tipos fechados do domínio foram consolidados.
- A estratégia de testes cobre as principais regras funcionais e critérios de desempate.
- O planejamento está suficientemente definido para iniciar a implementação.

### Lições aprendidas

O planejamento da suíte mostrou que testes devem validar o comportamento definido pela especificação funcional, e não detalhes da implementação. A separação entre testes unitários e de integração contribui para uma validação mais clara e organizada do sistema.

### Próximo passo

Iniciar a implementação do domínio, desenvolvendo cada componente acompanhado pelos testes correspondentes de maior prioridade.

---

## Sessão 8 — Estrutura inicial e componentes básicos do domínio

### Objetivo

Iniciar a implementação incremental da solução, criando a estrutura física dos projetos e implementando os componentes mais básicos e independentes do domínio.

### Resultado

A solution foi criada com os quatro projetos aprovados, as referências foram configuradas e os primeiros componentes do domínio foram implementados e validados por testes unitários.

### Atividades realizadas

- Criação da solution `DroneDeliveryCase`;
- Criação dos projetos:
  - `DroneDelivery.Domain`;
  - `DroneDelivery.Api`;
  - `DroneDelivery.Domain.Tests`;
  - `DroneDelivery.Api.IntegrationTests`;
- Configuração das referências entre os projetos;
- Implementação de `Coordinate`;
- Implementação de `DistanceCalculator`;
- Implementação dos enums `Priority` e `ImpossibleReason`;
- Criação e execução dos testes unitários correspondentes;
- Validação do build completo da solution.

### Decisões tomadas

- `Coordinate` foi implementado como `readonly record struct`;
- coordenadas não finitas são rejeitadas com `ArgumentOutOfRangeException`;
- `DistanceCalculator` foi implementado como classe estática e sem estado;
- o cálculo de distância não realiza arredondamento;
- os enums não possuem valores neutros ou genéricos;
- os testes dos enums verificam apenas os valores aprovados, sem depender da ordem numérica;
- a implementação continuará de forma incremental, com uma responsabilidade por etapa.

### Principais conclusões

- A estrutura da solution está funcional;
- as referências entre projetos estão corretas;
- o domínio já possui uma base matemática e estrutural validada;
- 17 testes do domínio estão aprovados;
- nenhuma dependência prematura de entidades ou algoritmos foi introduzida.

### Lições aprendidas

- Validar a estrutura da solution antes da lógica reduz o risco de erros acumulados;
- componentes independentes devem ser implementados primeiro;
- testes pequenos facilitam a identificação de falhas;
- enums não devem ser usados implicitamente como regra de ordenação.

### Próximo passo

Implementar a entidade `Drone` e seus testes unitários, mantendo o padrão incremental adotado.

---

## Sessão 9 — Implementação da entidade `Drone`

### Objetivo

Implementar a primeira entidade do domínio, garantindo a criação de objetos sempre em estado válido e validando suas invariantes por meio de testes unitários.

### Resultado

A entidade `Drone` foi implementada de forma imutável, com validações realizadas no construtor para impedir estados inválidos. Todas as regras definidas durante o planejamento foram verificadas por testes unitários, mantendo a implementação consistente com o modelo de domínio aprovado.

### Atividades realizadas

- Criação da entidade `Drone`;
- Implementação das propriedades `Id`, `CapacityKg`, `RangeKm` e `InputOrder`;
- Implementação das validações do construtor;
- Validação de identificadores obrigatórios;
- Validação de capacidade e alcance como valores positivos e finitos;
- Validação da ordem de entrada;
- Criação dos testes unitários da entidade;
- Execução dos testes do domínio;
- Validação do build completo da solution.

### Decisões tomadas

- `Drone` foi implementado como uma `class` imutável;
- todas as propriedades são somente leitura;
- o construtor impede a criação de objetos inválidos;
- identificadores nulos, vazios ou compostos apenas por espaços são rejeitados;
- capacidade e alcance devem ser valores finitos maiores que zero;
- `InputOrder` deve ser maior ou igual a zero;
- foram utilizadas apenas exceções padrão do .NET para validação dos argumentos;
- as validações permaneceram explícitas na própria entidade, sem introduzir classes auxiliares de validação.

### Principais conclusões

- A primeira entidade do domínio foi implementada conforme o modelo conceitual aprovado;
- as invariantes do objeto são garantidas durante sua criação;
- o padrão de implementação adotado poderá ser reutilizado nas próximas entidades;
- a suíte de testes do domínio passou a conter **32 testes aprovados**;
- a solution permanece compilando sem erros.

### Lições aprendidas

Implementar entidades imutáveis com validações concentradas no construtor simplifica a garantia das invariantes do domínio e reduz a possibilidade de criação de objetos inconsistentes. A implementação incremental, acompanhada de testes unitários, facilita a identificação de problemas e estabelece uma base confiável para as próximas etapas do algoritmo.

### Próximo passo

Implementar a entidade `Order`, reutilizando os componentes já desenvolvidos (`Coordinate` e `Priority`) e mantendo o mesmo padrão de validação, imutabilidade e cobertura por testes unitários.

---

## Sessão 10 — Implementação da entidade `Order`

### Objetivo

Implementar a entidade responsável por representar um pedido individual do sistema, garantindo sua imutabilidade e impedindo a criação de pedidos com dados inválidos.

### Resultado

A entidade `Order` foi implementada com propriedades somente para leitura e validações realizadas no construtor. A implementação reutiliza os componentes `Coordinate` e `Priority`, mantendo as responsabilidades já definidas no modelo de domínio.

Todos os testes unitários da entidade foram executados com sucesso, e a suíte do domínio passou a conter **44 testes aprovados**.

### Atividades realizadas

- Criação da entidade `Order`;
- implementação das propriedades `Id`, `WeightKg`, `Destination`, `Priority` e `InputOrder`;
- reutilização do value object `Coordinate` para representar o destino;
- reutilização do enum `Priority`;
- implementação das validações do construtor;
- validação do identificador obrigatório;
- validação do peso como um valor positivo e finito;
- validação da prioridade recebida;
- validação da ordem de entrada;
- criação dos testes unitários da entidade;
- execução isolada dos testes de `Order`;
- execução completa dos testes do domínio;
- validação do build da solution.

### Decisões tomadas

- `Order` foi implementada como uma `class` imutável;
- todas as propriedades são somente para leitura;
- o destino é representado por `Coordinate`, evitando manter coordenadas separadas na entidade;
- as validações já garantidas por `Coordinate` não são repetidas em `Order`;
- o valor padrão de `Coordinate`, correspondente à posição `(0, 0)`, é aceito como destino válido;
- o identificador não pode ser nulo, vazio ou composto apenas por espaços;
- o peso deve ser um número finito maior que zero;
- a prioridade deve corresponder a um valor definido no enum `Priority`;
- `InputOrder` deve ser maior ou igual a zero;
- foram utilizadas exceções padrão do .NET;
- nenhuma regra de comparação, ordenação ou seleção de pedidos foi adicionada à entidade.

### Principais conclusões

- A entidade `Order` foi implementada conforme o modelo de domínio aprovado;
- suas invariantes são protegidas durante a construção;
- o uso de `Coordinate` tornou o modelo mais expressivo e evitou duplicação de conceitos;
- a validação explícita de `Priority` protege o domínio contra conversões inválidas de valores numéricos;
- a suíte de testes passou de **32 para 44 testes aprovados**;
- a solution continuou compilando sem erros.

### Lições aprendidas

A reutilização de value objects e enums já validados reduz duplicação de regras e deixa as entidades mais simples. Cada componente deve validar apenas as invariantes que pertencem à sua própria responsabilidade.

Também foi reforçada a importância de proteger o domínio contra valores que podem ser aceitos pelo compilador, mas que não representam estados válidos, como a conversão direta de um número não definido para um enum.

### Próximo passo

Implementar a entidade `Trip`, utilizando uma coleção somente para leitura de pedidos, realizando uma cópia defensiva da coleção recebida e mantendo as regras de capacidade, alcance e planejamento fora da entidade.

---

## Sessão 11 — Implementação da entidade `Trip`

### Objetivo

Implementar a entidade responsável por representar uma viagem, preservando a imutabilidade da coleção de pedidos e garantindo apenas as invariantes estruturais do objeto.

### Resultado

A entidade `Trip` foi implementada utilizando uma coleção somente para leitura, protegida por cópia defensiva. A entidade valida apenas sua consistência estrutural, mantendo as regras de capacidade, alcance e planejamento sob responsabilidade do `TripPlanner`.

A suíte do domínio passou a conter **61 testes aprovados**.

### Atividades realizadas

- Implementação da entidade `Trip`;
- criação da coleção somente leitura de pedidos;
- implementação da cópia defensiva da coleção recebida;
- validação dos argumentos do construtor;
- implementação dos testes unitários;
- execução completa da suíte;
- validação do build.

### Decisões tomadas

- `Trip` foi implementada como classe imutável;
- pedidos são expostos como `IReadOnlyList<Order>`;
- a coleção recebida é copiada antes de ser armazenada;
- coleções vazias não são permitidas;
- a entidade não valida capacidade, alcance ou consistência do planejamento.

### Principais conclusões

- O domínio passou a representar viagens completas;
- a responsabilidade entre entidade e algoritmo permaneceu bem definida;
- a suíte atingiu **61 testes aprovados**.

### Lições aprendidas

Coleções imutáveis reduzem o acoplamento e evitam alterações externas em objetos que representam resultados do domínio.

### Próximo passo

Implementar a entidade `ImpossibleOrder`.

---

## Sessão 12 — Implementação da entidade `ImpossibleOrder`

### Objetivo

Representar pedidos que não podem ser atendidos juntamente com o motivo da impossibilidade.

### Resultado

Foi implementada a entidade `ImpossibleOrder`, contendo o pedido e seu motivo de impossibilidade. A implementação mantém o padrão de imutabilidade utilizado pelas demais entidades do domínio.

A suíte passou a conter **67 testes aprovados**.

### Atividades realizadas

- Implementação da entidade;
- validação do pedido recebido;
- validação do enum `ImpossibleReason`;
- criação dos testes unitários;
- execução da suíte completa.

### Decisões tomadas

- A entidade apenas representa um resultado;
- não verifica se o motivo informado está correto;
- a validação da lógica permanece responsabilidade do `TripPlanner`.

### Principais conclusões

- Todos os conceitos básicos do domínio passaram a estar representados;
- a suíte atingiu **67 testes aprovados**.

### Lições aprendidas

Nem toda entidade deve conter regras de negócio complexas. Algumas existem apenas para representar estados válidos do domínio.

### Próximo passo

Implementar o agregado `PlanningResult`.

---

## Sessão 13 — Implementação do agregado `PlanningResult`

### Objetivo

Implementar o agregado responsável por representar o resultado completo do planejamento das viagens.

### Resultado

Foi implementado o agregado `PlanningResult`, contendo as viagens planejadas e os pedidos impossíveis.

As coleções são expostas somente para leitura, protegidas por cópia defensiva e validadas estruturalmente.

A suíte passou a conter **79 testes aprovados**.

### Atividades realizadas

- Implementação do agregado;
- implementação das coleções somente leitura;
- cópia defensiva das coleções;
- validação dos argumentos;
- implementação dos testes unitários;
- execução completa da suíte.

### Decisões tomadas

- O agregado não valida regras do algoritmo;
- coleções vazias são permitidas;
- apenas integridade estrutural é verificada.

### Principais conclusões

- Todo o modelo de domínio passou a estar implementado;
- a suíte atingiu **79 testes aprovados**.

### Lições aprendidas

Agregados não precisam repetir regras cuja responsabilidade pertence ao componente que os produz.

### Próximo passo

Implementar o algoritmo de roteamento utilizando a heurística do Vizinho Mais Próximo.

---

## Sessão 14 — Implementação do algoritmo de roteamento

### Objetivo

Implementar o cálculo da rota utilizando a heurística do Vizinho Mais Próximo e retornar a sequência ordenada de pedidos juntamente com a distância total da viagem.

### Resultado

Foram implementados `RouteCalculationResult` e `NearestNeighborRouteCalculator`.

O algoritmo inicia a rota na base `(0,0)`, seleciona iterativamente o pedido mais próximo da posição atual e inclui explicitamente o retorno à base no cálculo da distância total.

Todos os critérios determinísticos de desempate definidos na especificação funcional foram implementados e validados.

A suíte completa do domínio passou a conter **100 testes aprovados**.

### Atividades realizadas

- Criação da pasta `Planning`;
- implementação de `RouteCalculationResult`;
- implementação de `NearestNeighborRouteCalculator`;
- utilização de `DistanceCalculator`;
- implementação da heurística do Vizinho Mais Próximo;
- implementação dos critérios determinísticos de desempate;
- implementação dos testes unitários;
- execução da suíte completa;
- validação do build.

### Decisões tomadas

- O algoritmo possui responsabilidade exclusivamente geométrica;
- drones, capacidade, alcance e pedidos impossíveis permanecem fora deste componente;
- a distância total inclui o retorno à base;
- a ordem de enumeração da coleção nunca é utilizada como critério de desempate;
- a prioridade é comparada explicitamente, sem depender do valor numérico do enum.

### Principais conclusões

- O roteamento tornou-se totalmente determinístico;
- todos os critérios definidos na especificação funcional foram implementados;
- o domínio passou a conter todos os componentes necessários para iniciar o planejamento das viagens;
- a suíte atingiu **100 testes aprovados**.

### Lições aprendidas

Separar o cálculo da rota do planejamento das viagens simplifica o domínio, aumenta a reutilização do algoritmo e torna os testes significativamente mais objetivos.

### Próximo passo

Iniciar a implementação incremental do `TripPlanner`, responsável por orquestrar todo o algoritmo de planejamento das viagens.

---

## Sessão 15 — Implementação inicial do `TripPlanner`

### Objetivo

Iniciar a implementação incremental do `TripPlanner`, estabelecendo a base do algoritmo de planejamento por meio das validações estruturais e dos primeiros comportamentos definidos na especificação funcional.

### Resultado

Foi implementada a primeira versão funcional do `TripPlanner`, responsável por validar os parâmetros de entrada, garantir a consistência das coleções recebidas e tratar os cenários-base do planejamento.

Nesta etapa ainda não foi implementada a lógica de formação de viagens ou seleção de pedidos, mantendo o escopo restrito às responsabilidades aprovadas para a primeira fase do algoritmo.

A suíte completa do domínio passou a conter **112 testes aprovados**.

### Atividades realizadas

- Criação da estrutura inicial do `TripPlanner`;
- implementação das validações de parâmetros nulos;
- implementação das validações de itens nulos nas coleções;
- implementação da validação de identificadores duplicados;
- implementação da validação de `InputOrder` duplicado;
- implementação do comportamento para coleções de pedidos vazias;
- implementação do comportamento para ausência de drones quando existem pedidos;
- criação dos testes unitários correspondentes;
- execução completa da suíte de testes;
- validação do build da solution.

### Decisões tomadas

- As coleções recebidas são materializadas no início da execução para evitar múltiplas enumerações;
- ausência de drones com pedidos existentes é tratada como entrada inválida, resultando em `ArgumentException`;
- pedidos impossíveis ainda não são identificados nesta etapa;
- nenhuma lógica de roteamento ou formação de viagens foi introduzida;
- o algoritmo permanece incremental, adicionando novas responsabilidades apenas nas próximas etapas.

### Principais conclusões

- O `TripPlanner` passou a possuir comportamento executável;
- todas as validações estruturais previstas para a primeira etapa foram implementadas;
- o domínio permaneceu consistente com a especificação funcional aprovada;
- a suíte atingiu **112 testes aprovados**.

### Lições aprendidas

Implementar primeiro as validações estruturais reduz a complexidade das etapas seguintes e estabelece uma base confiável para evolução do algoritmo. Cada incremento adiciona apenas uma responsabilidade nova, mantendo o comportamento sempre consistente e totalmente testado.

### Próximo passo

Implementar a identificação de pedidos impossíveis e iniciar a primeira versão da formação de viagens utilizando os componentes de planejamento já desenvolvidos.

---

## Sessão 16 — Identificação de pedidos impossíveis

### Objetivo

Ampliar o `TripPlanner` para identificar pedidos individualmente inviáveis antes da formação das viagens, classificando corretamente o motivo da impossibilidade conforme as restrições de peso e alcance da frota.

### Resultado

Foi implementada a identificação de pedidos impossíveis, considerando a distância de ida e volta entre a base e o destino e classificando cada pedido como:

- `WeightExceeded`;
- `RangeExceeded`;
- `WeightAndRangeExceeded`.

Também foi adotada a regra de que um pedido somente é considerado viável quando um mesmo drone atende simultaneamente aos requisitos de peso e alcance. Caso essas capacidades existam apenas em drones diferentes, o pedido permanece impossível.

A suíte completa passou a conter **118 testes aprovados**.

### Atividades realizadas

- Implementação da identificação de pedidos impossíveis;
- cálculo do alcance necessário considerando ida e volta;
- implementação da classificação dos motivos de impossibilidade;
- tratamento do cenário em que todos os pedidos são impossíveis;
- manutenção do `NotSupportedException` quando ainda existem pedidos viáveis aguardando a implementação da formação das viagens;
- criação dos testes unitários para todos os cenários de classificação;
- validação completa da suíte de testes e do build da solution.

### Decisões tomadas

- A viabilidade de um pedido depende da existência de um único drone capaz de atender simultaneamente aos requisitos de peso e alcance.
- A existência de capacidade em um drone e alcance em outro não torna o pedido viável.
- Quando todos os pedidos são impossíveis, o `PlanningResult` já é retornado corretamente.
- Quando ainda existem pedidos viáveis, a formação das viagens permanece pendente e o método continua lançando `NotSupportedException`.

### Principais conclusões

- O `TripPlanner` passou a executar uma etapa real do algoritmo de planejamento.
- A identificação de pedidos inviáveis tornou-se independente da futura formação das viagens.
- A regra de classificação ficou completamente coberta por testes automatizados.

### Lições aprendidas

Separar a identificação de pedidos impossíveis da lógica de formação das viagens reduz a complexidade do algoritmo e elimina a necessidade de reavaliar pedidos cuja inviabilidade depende apenas das características da frota disponível.

### Próximo passo

Implementar a primeira formação de viagens para os pedidos viáveis, iniciando pela seleção do drone e do primeiro pedido da rota.

---

## Sessão 17 — Formação da primeira viagem

### Objetivo

Implementar o primeiro cenário completo de planejamento de entregas, permitindo que o `TripPlanner` forme uma viagem quando existir exatamente um pedido viável após a identificação dos pedidos impossíveis.

### Resultado

Foi implementada a primeira formação de viagem do algoritmo. Quando existe exatamente um pedido viável, o `TripPlanner` seleciona o drone conforme os critérios definidos pela especificação, cria uma viagem contendo esse único pedido, utiliza o `NearestNeighborRouteCalculator` para calcular a rota e retorna um `PlanningResult` completo juntamente com os pedidos classificados como impossíveis.

Quando houver mais de um pedido viável, o método continua lançando `NotSupportedException`, preservando a implementação incremental do algoritmo.

A suíte completa passou a conter **125 testes aprovados**.

### Atividades realizadas

- Implementação da seleção do drone segundo os critérios da especificação;
- implementação da formação de uma viagem unitária;
- integração da viagem com o `NearestNeighborRouteCalculator`;
- retorno conjunto de viagens planejadas e pedidos impossíveis;
- atualização dos testes que anteriormente esperavam `NotSupportedException` para o cenário de um único pedido viável;
- criação de novos testes para validar a formação da viagem e a seleção do drone;
- execução completa da suíte de testes e validação do build.

### Decisões tomadas

- A viagem unitária reutiliza integralmente o `NearestNeighborRouteCalculator`, evitando duplicação da lógica de cálculo de rota.
- A seleção do drone permanece um detalhe interno do `TripPlanner`, validado exclusivamente pelos testes do comportamento público.
- A seleção do primeiro pedido ainda não foi generalizada, pois nesta etapa existe apenas um pedido viável.
- A formação de múltiplas viagens e a inserção incremental permanecem fora do escopo.

### Principais conclusões

- O `TripPlanner` passou a executar um fluxo completo de planejamento para um subconjunto da especificação.
- O algoritmo evoluiu sem introduzir abstrações antecipadas ou código morto.
- A evolução incremental continua preservando cobertura completa por testes automatizados.

### Lições aprendidas

Implementar primeiro o menor cenário completo de planejamento simplifica a evolução do algoritmo. A partir dessa base, as próximas etapas poderão expandir naturalmente para múltiplos pedidos e múltiplas viagens sem necessidade de refatorações estruturais.

### Próximo passo

Implementar a formação de viagens com múltiplos pedidos viáveis, introduzindo a seleção do primeiro pedido e a inserção incremental de candidatos na rota.

---

## Sessão 18 — Formação da primeira viagem com múltiplos pedidos

### Objetivo

Expandir o `TripPlanner` para formar a primeira viagem contendo múltiplos pedidos viáveis, reutilizando o algoritmo de roteamento e respeitando as restrições de capacidade e alcance durante todo o processo de planejamento.

### Resultado

Foi implementada a formação incremental da primeira viagem. O algoritmo passa a selecionar um pedido inicial, simular a inclusão de todos os pedidos restantes, recalcular a rota utilizando o `NearestNeighborRouteCalculator` e incorporar iterativamente o candidato que produz o menor aumento de distância, desde que respeite os limites do drone.

Quando todos os pedidos viáveis são atendidos na primeira viagem, o `PlanningResult` é retornado normalmente. Caso permaneçam pedidos viáveis, o comportamento continua sendo lançar `NotSupportedException`, preservando a implementação incremental do algoritmo.

A suíte completa passou a conter **130 testes aprovados**.

### Atividades realizadas

- Implementação da seleção do primeiro pedido;
- adaptação da seleção do drone ao novo fluxo;
- implementação da formação incremental da primeira viagem;
- simulação de todos os candidatos restantes;
- validação de peso e alcance a cada inserção;
- recálculo da rota após cada inclusão utilizando o `NearestNeighborRouteCalculator`;
- criação dos testes unitários para múltiplos pedidos;
- execução completa da suíte de testes;
- validação do build da solution.

### Decisões tomadas

- A formação da viagem reutiliza integralmente o `NearestNeighborRouteCalculator`;
- o candidato escolhido é aquele que produz o menor aumento de distância;
- não foi definida uma regra de desempate para candidatos com o mesmo aumento de distância;
- a implementação permanece restrita à formação da primeira viagem;
- pedidos impossíveis continuam sendo retornados juntamente com o resultado do planejamento.

### Principais conclusões

- O `TripPlanner` passou a planejar viagens com múltiplos pedidos;
- a lógica de formação da viagem permaneceu isolada da futura formação de múltiplas viagens;
- a evolução incremental do algoritmo foi preservada;
- a suíte atingiu **130 testes aprovados**.

### Lições aprendidas

A implementação incremental permitiu expandir o algoritmo sem introduzir código não exercitado pelos testes. Ao concentrar a lógica de construção da viagem em uma única responsabilidade, a futura implementação do planejamento de múltiplas viagens poderá reutilizar esse comportamento com poucas alterações.

### Próximo passo

Implementar o laço externo do planejamento para formar todas as viagens necessárias até consumir todos os pedidos viáveis, reutilizando a lógica de formação da primeira viagem desenvolvida nesta etapa.

---

## Sessão 19 — Planejamento de múltiplas viagens

### Objetivo

Expandir o `TripPlanner` para planejar todas as viagens necessárias até atender todos os pedidos viáveis, reutilizando a lógica de formação de viagem implementada anteriormente.

### Resultado

Foi implementado o laço externo de planejamento, permitindo que o algoritmo forme sucessivas viagens até consumir todos os pedidos viáveis. A lógica de construção de uma viagem foi reutilizada integralmente, passando apenas a operar sobre o conjunto de pedidos ainda não planejados.

O `NotSupportedException` anteriormente utilizado quando restavam pedidos viáveis deixou de existir, sendo substituído pela criação automática de novas viagens.

A suíte completa passou a conter **134 testes aprovados**.

### Atividades realizadas

- Implementação do laço externo do planejamento;
- reutilização da lógica de formação de viagens;
- remoção dos pedidos planejados ao final de cada iteração;
- reutilização dos drones entre viagens;
- remoção do `NotSupportedException` para pedidos viáveis remanescentes;
- atualização dos testes que anteriormente esperavam exceção;
- criação de testes para múltiplas viagens;
- validação da reutilização do mesmo drone;
- validação de que cada pedido viável é entregue exatamente uma vez;
- execução completa da suíte de testes;
- validação do build da solution.

### Decisões tomadas

- A lógica de formação de viagens permaneceu concentrada em `BuildTrip`;
- o laço externo apenas coordena a criação sucessiva das viagens;
- o mesmo drone pode ser reutilizado em viagens diferentes;
- pedidos impossíveis continuam sendo retornados separadamente no `PlanningResult`;
- cada iteração remove do conjunto de trabalho todos os pedidos já planejados, garantindo progresso do algoritmo.

### Principais conclusões

- O `TripPlanner` passou a implementar o fluxo completo de planejamento do MVP;
- todas as viagens necessárias são geradas automaticamente;
- cada pedido viável é planejado exatamente uma vez;
- o algoritmo permaneceu incremental e reutilizou integralmente os componentes desenvolvidos nas etapas anteriores;
- a suíte atingiu **134 testes aprovados**.

### Lições aprendidas

Separar a construção de uma viagem da coordenação do planejamento simplificou significativamente a evolução do algoritmo. A reutilização dos componentes implementados anteriormente permitiu adicionar suporte a múltiplas viagens sem duplicação de lógica, mantendo o código mais simples, previsível e fácil de testar.

### Próximo passo

Realizar os refinamentos finais do algoritmo, revisar casos de borda, ampliar a cobertura de testes quando necessário e iniciar a implementação da API para exposição do planejamento de viagens.

---

## Sessão 20 — Consolidação do TripPlanner (Bloco 1 e Bloco 2)

### Objetivo

Realizar a primeira etapa de consolidação do domínio, reduzindo a complexidade interna do `TripPlanner` e aprimorando sua organização, sem alterar qualquer regra de negócio ou comportamento já validado pelos testes.

### Resultado

O `TripPlanner` foi reorganizado por meio de refatorações estruturais, mantendo exatamente o mesmo comportamento funcional. A lógica de orquestração do planejamento foi extraída para métodos privados com responsabilidades bem definidas, tornando o fluxo principal mais simples e legível.

Também foi realizada uma revisão de organização da classe, padronizando a ordem lógica dos métodos e pequenas melhorias de consistência, sem introduzir novas abstrações ou aumentar a complexidade do projeto.

A suíte permaneceu com **134 testes aprovados**, confirmando que todas as alterações foram exclusivamente estruturais.

### Atividades realizadas

- Extração da lógica de formação de viagens para o método `BuildTrips`;
- extração da criação de uma viagem para o método `CreateTrip`;
- simplificação do método `Plan`, mantendo apenas a orquestração de alto nível;
- reorganização dos métodos privados conforme o fluxo natural de execução;
- revisão da nomenclatura das variáveis relacionadas às rotas e coleções;
- pequenos ajustes de legibilidade em `BuildTrip`;
- revisão geral da organização do `TripPlanner`;
- execução completa da suíte de testes;
- validação do build da solution.

### Decisões tomadas

- A refatoração não alterou nenhuma regra de negócio;
- nenhuma nova abstração (Factory, Strategy, Builder ou Application Layer) foi introduzida;
- métodos auxiliares só foram extraídos quando representavam conceitos claros do domínio;
- pequenas extrações sem ganho real de legibilidade foram descartadas;
- o foco permaneceu na clareza do código e na facilidade de manutenção.

### Principais conclusões

- O método `Plan` passou a representar apenas o fluxo de alto nível do planejamento;
- as responsabilidades ficaram melhor distribuídas entre métodos privados;
- a leitura da classe tornou-se mais próxima da sequência lógica do algoritmo;
- a consolidação preservou integralmente o comportamento validado pela suíte de testes;
- a arquitetura do domínio encontra-se preparada para a implementação da API.

### Lições aprendidas

Refatorar não significa criar novas abstrações ou reduzir o número de linhas de código. Nesta etapa, o principal objetivo foi melhorar a organização interna da classe, tornando cada método responsável por um único conceito do domínio. A validação por meio dos **134 testes automatizados** confirmou que a melhoria ocorreu apenas na estrutura do código, sem impacto funcional.

### Próximo passo

Revisar a suíte de testes em busca de redundâncias, inconsistências de nomenclatura e oportunidades de organização antes de iniciar a implementação da API ASP.NET Core.

---

## Sessão 21 — Consolidação da suíte de testes (Nomenclatura)

### Objetivo

Revisar a nomenclatura dos testes do `TripPlanner` para garantir que cada caso de teste descrevesse claramente o cenário exercitado e o comportamento esperado, mantendo consistência em toda a suíte.

### Resultado

Foi realizada uma revisão completa da nomenclatura dos testes do `TripPlanner`. A análise mostrou que a suíte já seguia, em sua maior parte, o padrão:

```text
Method_WhenCondition_ShouldExpectedBehavior
```

Foram realizadas apenas renomeações pontuais em testes cuja descrição poderia ser mais objetiva, sem alterar qualquer comportamento, asserção ou cobertura da suíte.

### Atividades realizadas

- revisão completa da nomenclatura dos testes;
- identificação de nomes excessivamente genéricos;
- substituição de descrições pouco específicas por nomes mais explícitos;
- preservação dos testes cuja nomenclatura já representava corretamente o comportamento esperado;
- execução completa da suíte de testes;
- validação do build da solution.

### Decisões tomadas

- renomear apenas testes com ganho claro de legibilidade;
- evitar alterações puramente estéticas;
- manter o padrão `Method_WhenCondition_ShouldExpectedBehavior` como referência para novos testes.

### Principais conclusões

- a suíte já apresentava alto nível de consistência;
- apenas poucos nomes precisaram de ajustes;
- os testes continuam funcionando como documentação executável do comportamento do domínio;
- nenhuma alteração funcional foi necessária.

### Validação

- ✅ 134 testes aprovados;
- ✅ build executado com sucesso;
- ✅ nenhuma alteração de comportamento.

### Lições aprendidas

Uma boa suíte de testes não depende apenas da cobertura, mas também da clareza dos seus nomes. Um teste bem nomeado comunica o cenário validado antes mesmo da leitura da implementação, tornando a suíte uma documentação viva do domínio.

### Próximo passo

Revisar a suíte em busca de possíveis duplicidades ou sobreposição de cenários, verificando se cada teste existente valida um comportamento distinto antes de iniciar a implementação da API.

---

## Sessão 22 — Consolidação da suíte de testes (Duplicidades)

### Objetivo

Revisar a suíte de testes do `TripPlanner` para identificar possíveis duplicidades, verificando se diferentes testes exerciam exatamente o mesmo comportamento e se havia oportunidades de simplificar a cobertura sem perda de proteção contra regressões.

### Resultado

Foi realizada uma revisão completa dos cenários cobertos pela suíte de testes. Embora alguns testes apresentem entradas semelhantes, todos exercitam regras de negócio distintas ou validam comportamentos complementares do algoritmo.

Nenhuma duplicidade real foi identificada e, consequentemente, nenhum teste precisou ser removido.

### Atividades realizadas

- revisão comparativa dos cenários de testes;
- análise das regras de negócio exercitadas por cada caso;
- verificação de sobreposição entre testes unitários e cenários agregados;
- avaliação da necessidade de remoção de testes redundantes;
- execução completa da suíte de testes;
- validação do build da solution.

### Decisões tomadas

- manter testes que utilizam entradas semelhantes, mas validam comportamentos diferentes;
- preservar testes específicos que isolam regras individuais do domínio;
- manter testes agregados responsáveis por validar a integração entre diferentes regras do algoritmo;
- não remover testes apenas por apresentarem cenários parecidos.

### Principais conclusões

- não foram encontradas duplicidades reais na suíte;
- a sobreposição existente é intencional e contribui para uma identificação mais precisa de regressões;
- os testes específicos e agregados se complementam, oferecendo uma cobertura mais robusta do comportamento do domínio;
- a organização atual da suíte permanece adequada para a manutenção do projeto.

### Validação

Após a revisão:

- ✅ nenhuma remoção de testes foi necessária;
- ✅ 134 testes aprovados;
- ✅ build executado com sucesso;
- ✅ nenhuma alteração de comportamento.

### Lições aprendidas

Cobertura elevada não significa necessariamente redundância. Em muitos casos, cenários semelhantes validam regras diferentes do domínio e facilitam a identificação da causa de uma regressão. Antes de remover testes, é importante verificar se eles realmente exercitam o mesmo comportamento ou apenas compartilham parte dos dados de entrada.

### Próximo passo

Realizar a etapa final da consolidação da suíte, verificando se existe alguma regra pública do domínio ainda sem cobertura antes de iniciar a implementação da API ASP.NET Core.

---

## Sessão 22 — Consolidação da suíte de testes

### Objetivo

Concluir a revisão da suíte de testes do `TripPlanner`, verificando sua organização, nomenclatura, possíveis duplicidades e a existência de regras públicas ainda sem cobertura explícita.

### Resultado

A suíte de testes foi revisada integralmente e considerada adequada para o escopo do desafio.

Foram realizados ajustes pontuais de nomenclatura para tornar alguns testes mais descritivos. Não foram identificadas duplicidades reais que justificassem remoções e nenhuma lacuna relevante de cobertura exigiu a criação de novos testes.

A suíte permaneceu com **134 testes aprovados**.

### Atividades realizadas

- revisão da organização do arquivo `TripPlannerTests`;
- análise da consistência dos nomes dos testes;
- renomeação pontual de testes com descrições genéricas;
- revisão de cenários potencialmente duplicados;
- comparação entre testes específicos e testes agregados;
- levantamento das regras públicas do `TripPlanner`;
- verificação da cobertura de validações, impossibilidades, seleção, roteamento e múltiplas viagens;
- análise dos critérios finais de desempate;
- execução completa da suíte de testes;
- validação do build da solution.

### Decisões tomadas

- manter a organização atual dos testes, por já acompanhar a evolução lógica do planejador;
- preservar o padrão de nomenclatura:

```text
Method_WhenCondition_ShouldExpectedBehavior
```

- renomear apenas testes com ganho claro de legibilidade;
- manter testes com entradas semelhantes quando protegem regras distintas;
- preservar testes específicos e agregados, pois exercitam níveis diferentes do comportamento;
- não remover testes apenas para reduzir o tamanho da suíte;
- não adicionar testes artificiais para métodos privados ou cenários de desempenho;
- não criar testes para desempates por `Id` quando o cenário dependeria de `InputOrder` duplicado, condição rejeitada pelas validações do domínio.

### Principais conclusões

- a suíte está organizada e utiliza nomes descritivos;
- nenhuma duplicidade real foi encontrada;
- a sobreposição existente é deliberada e saudável;
- todas as regras públicas alcançáveis do `TripPlanner` estão protegidas;
- os testes permanecem focados no comportamento público, sem acoplamento aos métodos privados;
- não foi necessário aumentar artificialmente o número de testes;
- a consolidação da suíte foi concluída com **134 testes aprovados**.

### Validação

- ✅ 134 testes aprovados;
- ✅ nenhuma falha;
- ✅ build executado com sucesso;
- ✅ nenhum teste removido;
- ✅ nenhum novo teste artificial adicionado;
- ✅ comportamento do domínio preservado.

### Lições aprendidas

Uma suíte de testes não deve ser avaliada apenas pela quantidade de casos existentes. É necessário verificar se cada teste protege uma regra relevante, se cenários semelhantes exercitam comportamentos distintos e se as regras públicas alcançáveis estão cobertas.

Também foi possível observar que nem todo critério documentado produz necessariamente um cenário válido e alcançável. Quando uma validação anterior impede que determinado desempate seja utilizado, criar um teste artificial para alcançá-lo introduziria uma contradição em vez de melhorar a cobertura.

A consolidação demonstrou que uma suíte pode ser considerada completa para o escopo do projeto sem buscar cobertura indiscriminada ou adicionar casos que não representam entradas válidas do domínio.

### Próximo passo

Iniciar a implementação da API ASP.NET Core, criando sua estrutura inicial e mantendo o domínio independente dos contratos e detalhes da camada HTTP.

---

## Sessão 23 — Encerramento da implementação do domínio

### Objetivo

Formalizar o encerramento da implementação e consolidação da camada de domínio antes do início da API ASP.NET Core.

### Resultado

A camada de domínio foi considerada concluída.

Todas as regras funcionais definidas durante a especificação foram implementadas, o algoritmo de planejamento encontra-se determinístico e a suíte de testes foi revisada quanto à organização, legibilidade, duplicidades e cobertura.

O domínio está preparado para ser consumido pela API sem depender de contratos HTTP ou detalhes de infraestrutura.

### Estado final do domínio

- modelos de domínio implementados e imutáveis;
- validações estruturais e invariantes protegidas;
- cálculo de distância isolado;
- roteamento por Vizinho Mais Próximo;
- critérios determinísticos de desempate;
- identificação e classificação de pedidos impossíveis;
- seleção de drones;
- seleção do primeiro pedido de cada viagem;
- inserção incremental de pedidos;
- formação de múltiplas viagens;
- reutilização dos drones;
- garantia de entrega única dos pedidos viáveis;
- separação entre viagens e pedidos impossíveis;
- refatoração estrutural do `TripPlanner`;
- suíte consolidada com **134 testes aprovados**.

### Consolidação realizada

#### Bloco 1 — Refatoração interna

- extração de `BuildTrips`;
- extração de `CreateTrip`;
- simplificação da orquestração do método `Plan`;
- preservação integral do comportamento.

#### Bloco 2 — Legibilidade

- revisão da organização dos métodos;
- revisão de nomes e variáveis;
- manutenção das estruturas que já estavam claras;
- rejeição de abstrações sem responsabilidade concreta.

#### Bloco 3 — Suíte de testes

- organização revisada;
- nomenclatura consolidada;
- nenhuma duplicidade real identificada;
- nenhuma lacuna relevante de cobertura;
- nenhuma remoção indevida;
- nenhuma expansão artificial da suíte.

### Principais conclusões

- o domínio atende integralmente ao escopo funcional do projeto;
- a arquitetura permanece simples, coesa e independente da camada HTTP;
- o comportamento está protegido por testes automatizados;
- as principais decisões de projeto encontram-se documentadas e validadas;
- não existem pendências conhecidas que impeçam o início da implementação da API.

### Validação

- ✅ implementação do domínio concluída;
- ✅ 134 testes automatizados aprovados;
- ✅ build executado com sucesso;
- ✅ comportamento preservado durante toda a consolidação.

### Lições aprendidas

Encerrar uma etapa de desenvolvimento exige mais do que finalizar a implementação. Foi necessário revisar a estrutura do código, confirmar a estabilidade do comportamento, avaliar criticamente a suíte de testes e evitar alterações que não agregassem valor ao projeto.

A consolidação realizada antes da API reduz o risco de misturar problemas do domínio com responsabilidades da camada HTTP, permitindo que a próxima fase seja dedicada exclusivamente à exposição dos comportamentos já validados.

### Próximo passo

Iniciar a implementação da API ASP.NET Core, começando pela estrutura do projeto, configuração inicial da aplicação e definição dos contratos de entrada e saída, mantendo a independência entre o domínio e a camada HTTP.

---

## Sessão 24 — Estrutura inicial da API ASP.NET Core

### Objetivo

Preparar a infraestrutura inicial da API ASP.NET Core para receber os endpoints do projeto, garantindo que a solução estivesse organizada, compilando corretamente e pronta para evoluir sem impactar a camada de domínio.

### Resultado

A estrutura base da API foi configurada com sucesso.

O projeto passou a referenciar apenas a camada de domínio, mantendo a arquitetura definida durante o desenvolvimento. Também foram realizados os ajustes necessários para utilizar o .NET 8 em toda a solução, eliminando inconsistências entre os projetos.

Ao final da etapa, a API iniciou corretamente e o Swagger foi disponibilizado para inspeção dos futuros endpoints.

### Atividades realizadas

- criação da estrutura inicial de pastas da API;
- organização das pastas `Controllers` e `Contracts`;
- configuração inicial do `Program.cs`;
- configuração dos serviços de Controllers;
- configuração do Swagger/OpenAPI;
- validação da referência entre `DroneDelivery.Api` e `DroneDelivery.Domain`;
- remoção dos arquivos de exemplo do template (quando aplicável);
- correção do `TargetFramework` dos projetos de testes para `net8.0`;
- recompilação completa da solução;
- execução da API e validação do Swagger.

### Decisões tomadas

- manter apenas uma dependência entre a API e o domínio;
- não introduzir novas camadas de aplicação;
- não adicionar bibliotecas externas nesta etapa (AutoMapper, MediatR, FluentValidation, etc.);
- manter o `Program.cs` simples, utilizando apenas os serviços necessários para inicialização da API;
- concluir a etapa apenas após validar a execução da aplicação.

### Validação

- ✅ solução compilando com sucesso;
- ✅ todos os projetos configurados para `net8.0`;
- ✅ API iniciada corretamente;
- ✅ Swagger disponível em `https://localhost:5027/swagger`;
- ✅ arquitetura preservada.

### Lições aprendidas

Antes de implementar funcionalidades, é importante garantir que toda a infraestrutura do projeto esteja consistente. Durante esta etapa foi identificada uma incompatibilidade entre o SDK utilizado e o `TargetFramework` dos projetos de teste, demonstrando a importância de validar a configuração da solução antes de iniciar o desenvolvimento da camada HTTP.

Com a infraestrutura estabilizada, as próximas etapas poderão se concentrar exclusivamente na implementação dos contratos e dos endpoints.

### Próximo passo

Iniciar a criação dos contratos (`Requests` e `Responses`) que serão utilizados pela API, mantendo a separação entre a camada HTTP e o domínio.

---

## Sessão 25 — Definição dos contratos públicos da API

### Objetivo

Definir conceitualmente os contratos de entrada e saída da API antes de iniciar sua implementação em C#.

### Resultado

Foram definidos os DTOs que formarão a interface pública da API, mantendo-os separados das entidades da camada de domínio.

Também foram congeladas as propriedades que serão expostas no JSON e as responsabilidades da futura camada de mapeamento.

### Contratos de entrada

#### `PlanningRequest`

- `Drones`
- `Orders`

#### `DroneRequest`

- `Id`
- `CapacityKg`
- `RangeKm`

#### `OrderRequest`

- `Id`
- `WeightKg`
- `Priority`
- `X`
- `Y`

### Contratos de saída

#### `PlanningResponse`

- `Trips`
- `ImpossibleOrders`

#### `TripResponse`

- `DroneId`
- `Orders`
- `TotalWeightKg`
- `TotalDistanceKm`

#### `TripOrderResponse`

- `Id`
- `Sequence`

#### `ImpossibleOrderResponse`

- `OrderId`
- `Reason`

#### `ErrorResponse`

- `Message`
- `Errors`

### Decisões tomadas

- manter os contratos da API separados das entidades de domínio;
- representar `Priority` como texto no contrato HTTP;
- representar `Reason` como texto na resposta;
- manter `Sequence` explícita em cada pedido da viagem;
- não expor `InputOrder` no JSON;
- inferir `InputOrder` pela posição dos elementos nos arrays recebidos;
- deixar a conversão dos contratos para o domínio sob responsabilidade da futura camada de mapeamento.

### Principais conclusões

O contrato público deve representar a intenção de quem consome a API, sem expor detalhes internos do algoritmo.

A ordem dos drones e pedidos nos arrays já fornece informação suficiente para gerar o `InputOrder`, evitando campos redundantes e possíveis inconsistências na requisição.

### Validação

- ✅ contratos de entrada definidos;
- ✅ contratos de saída definidos;
- ✅ propriedades públicas aprovadas;
- ✅ nenhuma implementação antecipada;
- ✅ domínio mantido independente da camada HTTP.

### Próximo passo

Definir o padrão de implementação dos DTOs em C#, comparando `class`, `record`, propriedades `init` e construtores posicionais antes de criar os arquivos dos contratos.

---

# Sessão 26 — Implementação dos contratos da API

## Objetivo

Implementar os contratos públicos da API definidos na etapa anterior, estabelecendo uma interface HTTP desacoplada da camada de domínio.

---

## Resultado

Todos os DTOs de Request e Response foram implementados seguindo o padrão arquitetural aprovado, mantendo a separação entre a camada HTTP e o domínio.

A solução foi recompilada com sucesso após a implementação, confirmando a consistência da estrutura criada.

---

## Contratos implementados

### Requests

- `PlanningRequest`
- `DroneRequest`
- `OrderRequest`

### Responses

- `PlanningResponse`
- `TripResponse`
- `TripOrderResponse`
- `ImpossibleOrderResponse`
- `ErrorResponse`

---

## Padrão adotado

Todos os contratos seguem a mesma convenção:

- `sealed record`;
- propriedades nomeadas;
- propriedades `init`;
- inicialização de referências com `string.Empty`;
- inicialização de coleções com `[]`;
- ausência de regras de negócio;
- ausência de validações;
- ausência de atributos de serialização.

Essa padronização proporciona consistência visual, imutabilidade após a desserialização e facilita futuras evoluções da camada HTTP.

---

## Decisões arquiteturais consolidadas

Durante esta etapa foram consolidadas as seguintes decisões:

- os contratos da API permanecem independentes das entidades do domínio;
- `Priority` será representada como texto no contrato HTTP;
- `Reason` será representada como texto nas respostas da API;
- `Sequence` será retornada explicitamente para indicar a ordem das entregas em cada viagem;
- `InputOrder` não faz parte do contrato público e será inferido pela posição dos elementos durante o mapeamento para o domínio.

---

## Validação

- ✅ oito DTOs implementados;
- ✅ estrutura de pastas organizada;
- ✅ padrão único aplicado a todos os contratos;
- ✅ solução compilando sem erros.

---

## Próximo passo

Implementar o primeiro endpoint da API (`PlanningController`), realizando o mapeamento entre os contratos HTTP e os objetos do domínio, preservando todas as regras de negócio já implementadas e testadas.

---

# Sessão 27 — Estrutura inicial do PlanningController

## Objetivo

Criar o primeiro endpoint da API, estabelecendo a rota de planejamento e conectando os contratos públicos de entrada e saída à camada HTTP.

Nesta etapa, o endpoint ainda não executa regras de negócio nem interage com o domínio.

---

## Implementação realizada

Foi criado o arquivo:

```text
src/DroneDelivery.Api/Controllers/PlanningController.cs
```

O controller expõe o endpoint:

```http
POST /api/planning
```

A operação recebe um `PlanningRequest` e retorna temporariamente um `PlanningResponse` vazio.

---

## Estrutura do endpoint

O controller utiliza:

- `[ApiController]`;
- `[Route("api/planning")]`;
- `[HttpPost]`;
- `ActionResult<PlanningResponse>`;
- `[ProducesResponseType]` para documentar a resposta de sucesso.

O ASP.NET Core infere automaticamente que o `PlanningRequest` deve ser desserializado a partir do corpo da requisição, sem necessidade de adicionar `[FromBody]`.

---

## Resposta temporária

Até que o mapeamento e a integração com o domínio sejam implementados, o endpoint retorna:

```json
{
  "trips": [],
  "impossibleOrders": []
}
```

Essa resposta temporária permite validar a infraestrutura HTTP sem antecipar responsabilidades da próxima etapa.

---

## Decisões arquiteturais

- manter o controller restrito à camada HTTP;
- não realizar ainda o mapeamento de DTOs para entidades de domínio;
- não instanciar ou chamar o `TripPlanner`;
- não implementar conversão de prioridades;
- não tratar exceções nesta etapa;
- não criar testes de integração ainda;
- utilizar `POST`, pois o planejamento depende de uma estrutura complexa enviada no corpo da requisição.

---

## Validação

- ✅ `PlanningController` criado;
- ✅ solução compilando sem erros;
- ✅ API iniciada corretamente;
- ✅ endpoint `POST /api/planning` visível no Swagger;
- ✅ requisição processada com status `200`;
- ✅ resposta temporária retornada com coleções vazias.

---

## Próximo passo

Implementar o mapeamento entre os contratos HTTP e os objetos do domínio, incluindo:

- criação dos drones;
- criação dos pedidos;
- geração de `InputOrder` pela posição nos arrays;
- conversão textual de `Priority`;
- criação de `Coordinate`;
- chamada ao `TripPlanner`;
- conversão do resultado de domínio para `PlanningResponse`.

---

---

# Sessão 28 — Mapeamento de Requests para o domínio

## Objetivo

Implementar a camada de mapeamento responsável por converter os contratos HTTP da API em objetos do domínio, preservando a separação entre a camada de apresentação e as regras de negócio.

---

## Resultado

Foi implementado o `PlanningMapper`, responsável por transformar os DTOs de entrada (`PlanningRequest`) em coleções de `Drone` e `Order` do domínio.

A implementação mantém o controller enxuto e centraliza toda a conversão entre os contratos HTTP e os modelos do domínio.

---

## Implementação realizada

Foi criada a pasta:

```text
src/DroneDelivery.Api/Mapping
```

e implementado o arquivo:

```text
PlanningMapper.cs
```

Foram implementados os métodos:

- `ToDomainDrones()`;
- `ToDomainOrders()`;
- `MapPriority()`.

---

## Decisões arquiteturais

- concentrar todo o mapeamento DTO → Domínio em uma classe estática;
- manter o `PlanningController` responsável apenas pelo fluxo HTTP;
- gerar `InputOrder` a partir da posição dos elementos nas coleções recebidas;
- converter `Priority` explicitamente por meio de um método dedicado (`MapPriority`);
- criar `Coordinate` durante o mapeamento, sem expor esse detalhe ao contrato HTTP;
- não adicionar qualquer regra de negócio ao mapper.

---

## Validação

- ✅ `PlanningMapper` implementado;
- ✅ conversão de drones implementada;
- ✅ conversão de pedidos implementada;
- ✅ conversão textual de prioridade implementada;
- ✅ solução compilando sem erros.

---

## Lições aprendidas

Centralizar o mapeamento entre contratos HTTP e modelos do domínio reduz o acoplamento do controller, melhora a organização da API e facilita futuras alterações tanto nos contratos quanto nas entidades do domínio, sem impactar a lógica de negócio.

---

## Próximo passo

Integrar o `PlanningController` ao `TripPlanner`, utilizando o `PlanningMapper` para converter os contratos de entrada e, posteriormente, implementar o mapeamento do `PlanningResult` para `PlanningResponse`.

---

---

## Sessão 29 — Mapeamento do resultado do domínio para a resposta da API

### Objetivo

Implementar o mapeamento responsável por converter o `PlanningResult` em `PlanningResponse`, concluindo o fluxo de conversão entre o domínio e a camada HTTP.

### Resultado

Foi implementado o mapeamento das respostas da API, convertendo viagens e pedidos impossíveis do domínio para seus respectivos DTOs. Com isso, o `PlanningMapper` passou a realizar a conversão completa nos dois sentidos entre os contratos HTTP e o domínio.

### Atividades realizadas

- Implementação do método `ToResponse`;
- implementação do método `ToTripResponse`;
- implementação do método `ToImpossibleOrderResponse`;
- geração da sequência das entregas durante o mapeamento;
- adaptação da implementação às propriedades reais do domínio;
- validação do build da solution.

### Decisões tomadas

- O mapeamento de resposta permanecerá centralizado em `PlanningMapper`;
- a API continuará responsável pela representação dos contratos HTTP;
- o domínio permanecerá independente de detalhes da camada de apresentação;
- nenhuma regra de negócio foi adicionada ao mapper.

### Principais conclusões

- O fluxo de conversão entre API e domínio foi concluído;
- o `PlanningMapper` passou a concentrar todo o mapeamento entre contratos HTTP e modelos do domínio;
- a arquitetura definida para a API foi preservada.

### Lições aprendidas

Adaptar o mapeamento às assinaturas reais do domínio reduz retrabalho e evita acoplamento entre a API e a implementação interna das entidades.

### Próximo passo

Conectar o `PlanningController` ao `TripPlanner`, substituindo a resposta temporária pela execução real do planejamento.

---

---

## Sessão 30 — Integração do controller com o planejamento

### Objetivo

Conectar o `PlanningController` ao `TripPlanner`, concluindo o fluxo funcional da API.

### Resultado

O endpoint `POST /api/planning` passou a converter a requisição para o domínio, executar o planejamento e retornar um `PlanningResponse` real.

### Atividades realizadas

- Integração do `PlanningMapper` ao controller;
- execução do `TripPlanner`;
- conversão do `PlanningResult` para os DTOs de resposta;
- validação do endpoint pelo Swagger;
- confirmação do cálculo de peso, distância e sequência das entregas.

### Decisões tomadas

- O `TripPlanner` continuará sendo instanciado diretamente no controller;
- não será criada uma interface sem necessidade concreta;
- a API continuará sem regras de negócio.

### Validação

- ✅ Build concluído;
- ✅ endpoint retornando `HTTP 200`;
- ✅ fluxo completo validado;
- ✅ resposta compatível com o resultado esperado.

### Próximo passo

Implementar o tratamento de erros e as respostas `400 Bad Request`.

---

---

## Sessão 31 — Tratamento de erros da API

### Objetivo

Implementar o tratamento de requisições inválidas, convertendo exceções do domínio em respostas HTTP 400.

### Resultado

O `PlanningController` passou a capturar exceções derivadas de `ArgumentException` e retornar um `ErrorResponse` padronizado para o cliente.

### Atividades realizadas

- Implementação do tratamento de exceções no controller;
- criação do método `CreateErrorResponse`;
- documentação das respostas `200` e `400` no Swagger;
- validação dos cenários de sucesso e erro.

### Decisões tomadas

- Utilização de um único `catch (ArgumentException)`;
- manutenção do tratamento local no controller;
- nenhuma infraestrutura adicional de tratamento global foi introduzida.

### Principais conclusões

- A API passou a responder adequadamente para requisições inválidas;
- o fluxo principal do controller permaneceu simples e legível;
- o comportamento esperado foi validado por testes manuais.

### Lições aprendidas

Traduzir exceções do domínio para respostas HTTP melhora a experiência do cliente da API sem comprometer a separação de responsabilidades.

### Próximo passo

Implementar os testes de integração para validar os principais cenários da API.

---

---

## Sessão 32 — Testes de integração da API

### Objetivo

Validar o comportamento do endpoint de planejamento por meio de testes de integração executando o pipeline HTTP completo.

### Resultado

Foram implementados testes cobrindo os principais cenários de sucesso, validação e impossibilidade de atendimento.

### Atividades realizadas

- Configuração do `WebApplicationFactory`;
- teste de requisição válida com validação do corpo da resposta;
- teste de prioridade inválida retornando `HTTP 400`;
- teste de peso inválido retornando `HTTP 400`;
- teste de pedido impossível retornando `HTTP 200`;
- validação completa com `dotnet test`.

### Decisões tomadas

- Os testes utilizam a API real em memória, sem mocks;
- apenas os principais comportamentos do contrato HTTP foram cobertos;
- regras detalhadas do algoritmo permaneceram nos testes unitários do domínio.

### Principais conclusões

- O fluxo HTTP completo foi validado;
- as respostas `200` e `400` estão funcionando conforme esperado;
- pedidos impossíveis são tratados como resultado de negócio, e não como erro da API.

### Próximo passo

Realizar a revisão final da solução, atualizar a documentação e preparar o encerramento do projeto.

---

## Sessão 33 — Planejamentos como recursos REST em memória

### Objetivo

Evoluir o planejamento para um recurso REST identificável e recuperável, utilizando armazenamento temporário em memória.

### Resultado

O endpoint `POST /api/planning` passou a:

- gerar um `planningId`;
- armazenar o cenário completo em memória;
- retornar `201 Created`;
- incluir o cabeçalho `Location`.

Também foi criado o endpoint:

```http
GET /api/planning/{planningId}
```

para recuperar um planejamento existente.

### Implementações

- criação de `StoredPlanningScenario`;
- criação de `InMemoryPlanningStore`;
- utilização de `ConcurrentDictionary`;
- registro do store como `Singleton`;
- criação de `PlanningCreatedResponse`;
- alteração do `POST` para retornar `201 Created`;
- implementação do `GET` por identificador;
- retorno de `404 Not Found` para IDs inexistentes;
- armazenamento de cópias defensivas de drones e pedidos;
- atualização da suíte de testes de integração.

### Decisões

- o armazenamento permanece exclusivamente em memória;
- os dados são perdidos ao reiniciar a aplicação;
- não foi adicionado banco de dados;
- o domínio e o `TripPlanner` permaneceram inalterados;
- nenhuma análise de frota ou dashboard foi implementada nesta etapa.

### Validação

- ✅ `POST` retorna `201 Created`;
- ✅ resposta contém `planningId` e `createdAtUtc`;
- ✅ cabeçalho `Location` aponta para o recurso criado;
- ✅ `GET` recupera corretamente um planejamento existente;
- ✅ identificadores inexistentes retornam `404 Not Found`;
- ✅ requisições inválidas continuam retornando `400 Bad Request`;
- ✅ pedidos impossíveis continuam sendo representados corretamente;
- ✅ todos os testes de integração aprovados.

### Próximo passo

Implementar os endpoints especializados:

```http
GET /api/planning/{planningId}/routes
GET /api/planning/{planningId}/drones
```

reutilizando o cenário armazenado, preparando a base para o módulo de análise da frota e o dashboard.

---

## Sessão 34 — Endpoints especializados de rotas e drones

### Objetivo

Expandir a API REST com endpoints especializados para consulta das rotas planejadas e do resumo de utilização da frota, reutilizando o cenário armazenado em memória.

### Resultado

Foram implementados dois novos endpoints:

```http
GET /api/planning/{planningId}/routes
GET /api/planning/{planningId}/drones
```

O primeiro retorna a sequência de entregas realizada em cada viagem, incluindo as coordenadas de cada parada. O segundo fornece um resumo operacional de todos os drones do planejamento, incluindo aqueles que não participaram de nenhuma entrega.

### Implementações

- criação dos contratos `RouteResponse`, `RouteStopResponse` e `DroneSummaryResponse`;
- implementação de `PlanningMapper.ToRouteResponses()`;
- implementação de `PlanningMapper.ToDroneSummaryResponses()`;
- inclusão dos endpoints `/routes` e `/drones` no `PlanningController`;
- reutilização do `StoredPlanningScenario` armazenado em memória;
- retorno de `404 Not Found` para planejamentos inexistentes;
- implementação de quatro novos testes de integração cobrindo sucesso e falha dos novos endpoints.

### Decisões

- os endpoints reutilizam exclusivamente os dados já armazenados em memória;
- nenhuma nova execução do `TripPlanner` é realizada durante as consultas;
- a sequência das paradas é derivada da ordem dos pedidos na viagem;
- as coordenadas são obtidas diretamente dos pedidos armazenados no cenário;
- o resumo da frota inclui drones utilizados e não utilizados;
- nenhuma lógica de análise da frota foi adicionada nesta etapa.

### Validação

- ✅ `GET /routes` retorna as rotas planejadas corretamente;
- ✅ `GET /drones` retorna o resumo completo da frota;
- ✅ planejamentos inexistentes retornam `404 Not Found`;
- ✅ todos os testes de integração aprovados;
- ✅ suíte completa de testes validada com sucesso.

### Próximo passo

Implementar o módulo de análise da frota (`FleetAdvisor`), responsável por gerar métricas operacionais e recomendações sobre utilização, capacidade e eficiência da frota.

---

## Sessão 35 — Implementação do módulo de análise da frota

### Objetivo

Implementar o módulo de análise da frota (`FleetAdvisor`), responsável por gerar métricas operacionais e recomendações a partir de um planejamento armazenado, mantendo a lógica desacoplada dos contratos HTTP.

### Resultado

Foi criada a estrutura de análise da frota baseada em modelos internos, permitindo calcular indicadores de utilização, eficiência e capacidade da frota sem depender da camada Web.

### Implementações

- criação do `FleetAdvisor`;
- criação dos modelos internos `FleetAnalysis`, `DroneAnalysis` e `FleetRecommendation`;
- criação de `FleetAnalysisOptions` para configuração dos parâmetros da análise;
- implementação dos cálculos de:
  - participação da frota;
  - fator médio de carga;
  - eficiência (kg/km);
  - tempo estimado das viagens;
  - utilização individual dos drones;
  - consumo médio e máximo de autonomia;
- geração de recomendações estruturadas (`Success`, `Information`, `Warning` e `Critical`);
- correção das referências para refletir as entidades reais do domínio (`Trip.Drone`, `Order.Destination` e `ImpossibleOrder.Order`).

### Decisões

- o `FleetAdvisor` retorna apenas modelos internos;
- o `PlanningMapper` permanece responsável pela conversão para os DTOs da API;
- os cálculos utilizam exclusivamente os dados do `StoredPlanningScenario`;
- as recomendações foram estruturadas em tipo, título e descrição, preparando a integração com o dashboard;
- os parâmetros de análise permanecem configuráveis via `FleetAnalysisOptions`.

### Validação

- ✅ métricas calculadas corretamente a partir do cenário armazenado;
- ✅ recomendações geradas conforme o estado da frota;
- ✅ referências ajustadas para o modelo de domínio;
- ✅ suíte completa de testes aprovada.

### Próximo passo

Implementar o endpoint:

```http
GET /api/planning/{planningId}/fleet-analysis
```

integrando o `FleetAdvisor` ao `PlanningMapper` para disponibilizar a análise completa por meio da API.

---

## Sessão 36 — Refinamento das recomendações da análise de frota

### Objetivo

Estabilizar o contrato da análise de frota antes da implementação do dashboard, tornando as recomendações mais expressivas, seguras e adequadas para consumo pela camada de apresentação.

### Resultado

As recomendações passaram a utilizar categoria e severidade como conceitos distintos. Internamente, esses valores são representados por enums, enquanto o contrato HTTP continua expondo strings amigáveis no JSON.

### Implementações

- criação do enum interno `RecommendationType`;
- criação do enum interno `RecommendationSeverity`;
- atualização do modelo `FleetRecommendation`;
- inclusão da propriedade `Severity` no `FleetRecommendationResponse`;
- atualização do `PlanningMapper` para converter os enums em strings;
- separação das recomendações de capacidade e alcance;
- revisão dos títulos e das descrições geradas pelo `FleetAdvisor`;
- manutenção dos campos opcionais de capacidade e alcance mínimos sugeridos para pedidos impossíveis.

### Categorias de recomendação

- `FleetUtilization`;
- `Capacity`;
- `Range`;
- `ImpossibleOrders`;
- `Performance`.

### Severidades

- `Success`;
- `Information`;
- `Warning`;
- `Critical`.

### Decisões

- `Type` representa o assunto ou a categoria da recomendação;
- `Severity` representa o nível de importância;
- os enums permanecem internos ao módulo de análise;
- os DTOs HTTP continuam utilizando strings;
- o `PlanningMapper` permanece responsável pela conversão para o contrato externo;
- carga e alcance geram recomendações independentes, evitando categorias ambíguas;
- o contrato da análise foi considerado estável para o desenvolvimento do dashboard.

### Validação

- ✅ projeto compilado com sucesso;
- ✅ recomendações de capacidade e alcance separadas corretamente;
- ✅ enums convertidos corretamente para strings no response;
- ✅ mensagens revisadas e orientadas à ação;
- ✅ testes existentes executados e aprovados.

### Próximo passo

Iniciar a **Etapa 6.4 — Dashboard**, criando uma interface estática com HTML, CSS e JavaScript para consumir os endpoints da API.