![N|Lowpoc](https://i.ibb.co/MGshCk7/Black-Technology-Linked-In.png)

# Antes de iniciar nossa jornada de conhecimento, mostrarei como ficará dividido esse contéudo:

| Steps
| ------
| Conceitos
| Expondo ferramentas envolvidas 
| Criando um cenário 
| Resolvendo o cenário 
| Referências
| Minhas Redes sociais 

# Conceitos  
    - O que é redis?
    
     R: É um projeto open source que tem como objetivo fornecer uma storage para armazenamento de dados que pode ser utilizado como cache ou message broker. Nele podemos armazenar em geral: Strings, conjunto de dados(Array,List) e etc.
     
    - O que é .NetCore?
    
     R: É um framework desenvolvido pela Microsoft que tem como base linguística: C#, F# por exemplo, além de poder ser utilizados nos sistemas operacionais: Windows, MacOS e distribuições Linux.
    - Qual o papel de um "CACHE"?
    
     R: Visa armazenar um conjunto de informações que são frequentemente utilizados afim de recuperá-los de forma mais rápida.
     
    - O que o Docker vai ajudar?
    
     R: A fornecer um conjunto de produtos de plataforma como serviços de forma isolada uns dos outros, trazendo transparência e facilidade na hora de rodar aplicação. 
     
     - Como funciona o Cache distribuído?
     
     R: Ele é armazenado em um conjunto de servidores(clusters), no qual tem como objetivo principal de melhorar o desempenho, escalabilidade e resiliência das informações.

# Expondo ferramentas envolvidas
   - [Instalar .Net Core 3.1 SDK](https://dotnet.microsoft.com/download)
   - [Instalar Docker [Windows, MacOs, Linux]](https://docs.docker.com/get-docker/)

# Criando um Cenário

> Imaginemos que temos uma aplicação que tem  como finalidade monitorar o tempo da sua localidade
> e ele possui uma api rest construida com .Net Core que está passando  por grandes instabilidades devido à 
> demora de receber os conteudos de um endpoint X.
> Esse mesmo endpoint é muito solicitado, conhecido como "CÓDIGO QUENTE", pois a cada 10 ms(milésimo de segundo) é feito um request, tornando o processo de monitoria inválido. Então a equipe chamou o grande "Developer Marcus" pra resolver HAHAH e assim foi feito!
> Vamos analisar o seguintes passos para entender como ele resolveu?

# Resolvendo o Cenário
 - Primeira etapa:
        -  Vamos realizar uma bateria de testes para entender como está a saúde desse nosso endpoint , para isso iremos utilizar uma ferramenta muito boa, porém pouco conhecida que é o [Runner do Postman](https://learning.postman.com/docs/running-collections/intro-to-collection-runs/), ideal para realizarmos o teste de requisição em massa no tempo informado lá em cima que é de "10ms".
        -  Já ia esquecendo, antes de rodar o runner, vamos configurar nosso endpoint lá no Postman.
        ![](https://i.ibb.co/wSRGG9J/Endpoint-Postman.png)
        - Analisando a imagem acima podemos perceber que nosso endpoint é [https://localhost:32784/weatherforecast] e alguns scripts de testes que fazem alguns checks que serão explicados nos próximos passos.
        - Vamos startar a aplicação utilizando o comando `` docker-compose up -d `` e magicamente utilizando o docker e docker compose subirá a aplicação .NetCore + Redis
        - Rode o ```comando docker container ls```, ele irá listar os serviços/produtos rodando dentro dos containers. Deve aparecer algo parecido com isso: 
        ![](https://i.ibb.co/KrHTH8d/Ls.png)
        - Como falei anteriormente, tinha criado três checks
    - Response Time: Valida o tempo aceitavel pelo endpoint
    - Body: Verifica se está retornando conteúdo no corpo do response.
    - Status: Verifica se está retornando 200.
    - Vamos abrir nosso runner e preencher os campos iterations com 100 e Delay 10, ou seja ocorrerá 1 requisição no intervalo de 10ms até chegar 100.
    - Resultado dessa primeira etapa foi: das 100 solicitações foram reprovadas 100% no check de "response time", porque a taxa de retorno é > 100ms, conforme escrito no teste que é a taxa aceitável pela empresa. ![](https://i.ibb.co/MN7rYJD/Check-Response.png)
    
 - Etapa 2:
     - Bom agora que sabemos que o ofensor de fato é a nossa latência, vamos aplicar nosso cache distribuido com redis.
     - Para isso criei algumas classes:

| Nome | Funcionalidade | Endereço
| ------ |  ------ | ------ |  
| ResponseCacheRedis | Responsável por abstrair as funcionalidades de salvar e resgatar o cache | [link](https://github.com/Lowpoc/NetCore-Redis/blob/master/UseCaseDistribuitedCacheWithRedis/Services/ResponseCacheRedis.cs)
| IResponseCacheRedis  | Interface que define os contratos a serem respeitados pelo ResponseCacheRedis | [link](https://github.com/Lowpoc/NetCore-Redis/blob/master/UseCaseDistribuitedCacheWithRedis/Interface/IResponseCacheRedis.cs)
| Cached | Um atributo/filter de action assíncrono, na qual será responsável por interceptar o request e identificar se tem  algo em cache ou não, assim reduzindo  drasticamente o tempo de resposta. | [link](https://github.com/Lowpoc/NetCore-Redis/blob/master/UseCaseDistribuitedCacheWithRedis/Filters/Cached.cs)
| Redis |  Responsável por aplicar as configurações base do redis: Connection String, Interface, Serviço. | [link](https://github.com/Lowpoc/NetCore-Redis/blob/master/UseCaseDistribuitedCacheWithRedis/Extensions/Redis.cs)

- Etapa 3:
    - Agora basta colocar dentro da nossa controller nosso atributo chamado "Cached" especial para ver a mágica acontecer!
    - ![](https://i.ibb.co/QvtZ5xr/Controller.png)
    - Vamos ver o o resultado:
    ![](https://i.ibb.co/jbcwgxX/Capturar.png)
    - WOW o resultado foi fantástico, dos nossos 100 checks de response time , tivemos apenas 7 erros ou seja uma melhoria de 93% de comparado com o resultado anterior. Agora nosso serviço de monitoramento poderá trabalhar de forma muita mais tranquila, pois o tempo de resposta saiu de 1000ms média para 35ms.

# Opiniões
   - O cache é uma ferramenta muito poderosa e que deve ser utilizada de forma estratégica nas nossas aplicações, afim de resolver alguns cenários de performance.
   - Nem todo cenário precisa de cache.
   - Existem outros métodos para melhorar performance, como por exemplo: Alocação no GC[Garbage Collection].
# Referências
   - [Redis](https://redis.io/)
   - [.NetCore](https://docs.microsoft.com/pt-br/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-3.1)
   - [Cache Distribuido com .NetCore](https://docs.microsoft.com/pt-br/aspnet/core/performance/caching/distributed?view=aspnetcore-3.1)
   - [Docker](https://www.docker.com/)

# Minhas Redes Sociais

   - [Linkedin](https://www.linkedin.com/in/marcus-vinicius-santana-silva-0a1602117/)
   - [Instagram](@olasoumarcus)
   - [Youtube](https://www.youtube.com/channel/UCwNHLO-2BAaIfuMxK_OERxw)
 

Copyright (c) 2020-present, [Marcus V.S. Silva [Lowpoc]](https://github.com/Lowpoc)
