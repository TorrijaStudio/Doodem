# Game Design Document (GDD)

**Juegos para Web y Redes Sociales**  
**Grado en Diseño y Desarrollo de Videojuegos**  
**Grupo F**  

- Mario González Mallenco
- Diego Martínez García
- Lenin Anderson Carrasco Romero
- Lucía Álvarez Blázquez
- Sancho Quesada Carballo
- Alejandro García Martínez  

_Octubre 2024_

---

# Game Design Document - Doodem

**Juegos para Web y Redes Sociales**  
Grado en Diseño y Desarrollo de Videojuegos  
Grupo F - Torrija Studio  

**Autores:**  
- Mario González Mallenco  
- Diego Martínez García  
- Lenin Anderson Carrasco Romero  
- Lucía Álvarez Blázquez  
- Sancho Quesada Carballo  
- Alejandro García Martínez  

**Fecha:** Octubre 2024  

---

## Índice

1. Concepto de juego  
   - 1.1 Introducción  
   - 1.2 Descripción  
   - 1.3 Puntos clave  
   - 1.4 Género  
   - 1.5 Plataforma  
   
2. Mecánicas  
   - 2.1 Core  
      - 2.1.1 Combinación  
      - 2.1.2 Terraformación  
   - 2.2 Tótems  
   - 2.3 Estadísticas y habilidades  
   - 2.4 Biomas  
      - 2.4.1 Recursos  
      - 2.4.2 Obstáculos  
   - 2.5 Estados  
      - 2.5.1 Positivos  
      - 2.5.2 Negativos  
   - 2.6 Comportamiento de personajes  
   - 2.7 Fases de la partida  
      - 2.7.1 Fase preparatoria  
      - 2.7.2 Fase de combate  
      - 2.7.3 Fase de compra  
   - 2.8 Economía  
      - 2.8.1 Dinero obtenido  
      - 2.8.2 Gastos  
   - 2.9 Ligas  
   - 2.10 Controles  
   - 2.11 Ejemplo de partida  

3. Arte  
   - 3.1 Estética general  
   - 3.2 Arte conceptual  
   - 3.3 Arte 3D  

4. Sonido y Música  
   - 4.1 Efectos de sonido  
   - 4.2 Música  

5. Interfaces y menús  
   - 5.1 Flow-Charts  
   - 5.2 Mockups  

6. Monetización y modelo de negocio  
   - 6.1 Modelo de negocio  
      - 6.1.1 Información del usuario  
      - 6.1.2 Mapa de empatía  
      - 6.1.3 Caja de herramientas  
      - 6.1.4 Modelo de lienzo  
   - 6.2 Monetización  
      - 6.2.1 Tienda  

---

## 1 Concepto de juego

### 1.1 Introducción
Doodem es un juego 1vs1 online de género auto battler en el que los jugadores deben
 crear criaturas con diferentes habilidades para que luchen contra las criaturas del rival.

### Descripción
 Doodem es un juego multijugador que enfrenta a dos jugadores en una batalla estratégi
ca a lo largo de diferentes fases. En las batallas se utilizarán tótems, cada uno de ellos
 compuesto por tres partes de un animal.
 Una vez dentro de la partida a cada jugador se le presentarán de forma aleatoria una serie
 de tótems, de entre los que deberá elegir para ir creando su equipo. Con estos tótems cada
 jugador puede formar diferentes criaturas mezclando las diferentes partes de los animales.
 Además de esto deberá tomar una serie de decisiones en cuanto a cómo utilizar los tótems,
 la forma de gastar y producir dinero dentro de la partida y cómo utilizar los biomas a su
 disposición para moldear el terreno de juego en su favor.
 Tras terminar la partida el jugador recibirá o perderá puntos de liga en función del resul
tado de la partida. El número total de puntos de liga que tenga un jugador determinará
 qué rango tiene en las ligas competitivas.

### 1.3 Puntos clave
- **Estrategia mediante combinación:**  La mecánica principal de Doodem es la creación
 de combinaciones de animales a modo de tótems para combatir. Con el número de
 tótems ideados para la salida del juego y el número de piezas necesarias para hacer
 un tótem completo (tres) surgen numerosas estrategias y formas de jugar.
- **Estrategia mediante terraformación:** Además de la colocación de los animales, losjugadores pueden colocar biomas en el tablero con el objetivo de potenciar a sus
 animales y limitar a los del bando rival, generando así un espacio de juego único.
- **Gran rejugabilidad por la variedad de partidas:** La gran variedad de combinaciones,así como las formas de colocar los diferentes obstáculos en el tablero, hacen que Doodem sea un juego muy rejugable y original, ya que no habrá dos partidas iguales, y la estrategia y estilo de juego de cada jugador afecta directamente al transcurso de los combates.
- **Posibilidad de expansión del juego:** Por el tipo de juego que es, Doodem tiene potencial para ir lanzando periódicamente expansiones temáticas que añadan nuevos
 tótems, como por ejemplo, una expansión de animales árticos, animales fantásticos,
 etc.

### 1.4 Género
Doodem es un juego de estrategia del género auto battler en el que dos jugadores se enfrentan en un duelo de criaturas. Los jugadores se limitan a colocar a sus animales y los biomas en el escenario. Una vez comenzado el combate, los animales pelean entre ellos de manera automática. Algunos juegos del mismo género son *Backpack Battles* o *Team Fight Tactics*.

### 1.5 Plataforma
Doodem es un juego de navegador que se puede jugar tanto en PC como en dispositivos inteligentes como móviles o tablets, a través de la página web de itch.io.

---

## 2. Mecánicas

### 2.1 Core
Las mecánicas centrales en *Doodem* son una mezcla entre la creación de criaturas mediante la combinación de tótems, y la personalización del terreno de juego para conseguir una ventaja sobre el contrincante.

El terreno de juego es un espacio dividido en dos zonas, la del jugador y la del rival.

#### 2.1.1 Combinación
Los tótems son el elemento más importante del juego, dado que son los elementos alrededor de los cuales el jugador debe crear una estrategia, y a través de los que interactúa con el mapa. Los tótems son figuras de animales que, al colocarse en el mapa, se convierten en los seres a los que representan. Estas criaturas pelearán contra las criaturas del contrincante, hasta que solo queden en pie las de un bando.

Los tótems están formados por tres piezas (cabeza, torso y base), que el jugador puede mezclar a su antojo con piezas de otros tótems. Estas creaciones siempre deben constar de exactamente una pieza de cada tipo, es decir: una cabeza, un torso y una base. El interés de mezclar distintos animales radica en la capacidad de dotar a tu criatura de distintas habilidades; por ejemplo, el caparazón de una tortuga le otorgará protección, mientras que las patas de un oso le proporcionan un gran poder de ataque.

#### 2.1.2 Terraformación
El terreno es el segundo elemento estratégico con el que crear diferentes estrategias durante el curso de la partida y está dividido en un tablero en el cual cada casilla vale un pie. Existen cinco biomas diferentes que serán los que den forma al espacio de juego. Cada uno de estos biomas tendrá diferentes recursos, como pueden ser los árboles del bioma de bosque, o las rocas del bioma de montaña. De esta manera, las criaturas formadas por los tótems deben recoger los recursos de los diferentes biomas para potenciar o activar algunas de sus habilidades.

Al comienzo de la partida el bioma de todo el espacio de juego será elegido de forma aleatoria. A lo largo de la partida los jugadores podrán alterar ciertas zonas para colocar los biomas que deseen, tanto en su propia zona como en la del enemigo de la siguiente manera:

- En la fase de tienda el jugador puede comprar varios tipos de biomas. 
- Una vez hecho esto, el jugador podrá elegir uno de esos biomas y seleccionar una casilla del terreno.
- Al hacer esto, tanto esa casilla como todas las que la rodeen en cierto radio serán convertidas en el bioma elegido.

Cualquier bioma que se coloque en la zona del propio jugador prevalecerá sobre cualquier bioma que coloque el oponente en esa misma zona. Es decir, si los dos jugadores colocan un bioma en el mismo lugar, prevalecerá el del jugador propietario de esa parte del mapa.

### 2.2 Tótems
Los tótems son la principal mecánica de *Doodem*, permitiendo a los jugadores crear combinaciones de animales con las que combatir. Los tótems representan animales reales, y en función de qué parte se use en la combinación, se obtienen unas estadísticas u otras.

En el juego base hay ocho tótems:

- Águila imperial ibérica
- Buitre Negro
- Oso Pardo Europeo
- Lince ibérico
- Abeja ibérica
- Tortuga mediterránea
- Lagarto bético

Cada parte de los tótems tiene una estadística de vida asociada. Además de esto, las habilidades pueden estar asignadas a cualquier parte del cuerpo, y pueden tener ciertas estadísticas asociadas como ataque, velocidad y recursos necesarios o proporcionar diferentes estados como volar o sanar, que se explicarán más adelante.

### 2.3 Estadísticas y habilidades
Todas las partes de tótem tienen dos tipos de estadística base: la vida y la velocidad.

- **Vida:** Indica cuantos puntos de daño puede recibir un tótem antes de morir. Todas las piezas de tótem tienen al menos 5 puntos de vida, por lo que la vida mínima que tendrá cualquier tótem será de 15 puntos.
- **Velocidad:** Está asociada tanto a la velocidad de movimiento como a la de ataque, y varía de 1 a 5. La velocidad máxima que podría obtener una criatura sería de 15, mientras que la velocidad mínima de cualquiera será siempre 3.

Cuando haya partes de animales diferentes en un tótem este obtendrá un bonus que aumentará aún más cuando las tres partes del tótem sean de animales distintos.

En cuanto a las habilidades, no todas las partes tendrán una. En su defecto, este hueco será reemplazado por un ataque, aunque pueden existir en una misma pieza tanto ataque como habilidad. Todas las habilidades de cada tótem entran dentro de una las diferentes categorías de habilidad, que definen cuándo se activan:

- **Al matar a un enemigo**
- **Al recolectar recursos**
- **Al morir**
- **Durante la ronda (pasiva)**

### 2.4 Biomas
El número inicial de biomas es cinco. De ellos, tres generan recursos útiles para las criaturas, mientras que los otros dos provocan efectos negativos. Los biomas que generan recursos lo hacen todos al mismo tiempo. Estos biomas también generan obstáculos físicos que retrasan el movimiento de las criaturas. Al inicio de la partida ningún bioma tiene recursos ni obstáculos generados.

#### 2.4.1 Recursos
Los recursos y obstáculos en todos los biomas se generan cuatro veces a lo largo de la partida, independientemente de la duración de la partida. Estos recursos durarán cierto tiempo hasta que desaparezcan.

En el caso de los biomas que generan efectos negativos, estos lo harán a lo largo de toda la partida constantemente.

#### 2.4.2 Obstáculos
Los obstáculos son similares a las paredes. Bloquean el paso de las criaturas, a menos que una habilidad diga lo contrario. Estos obstáculos pueden tener diferentes formas, cubriendo diferentes espacios basados en la cuadrícula de juego. Los obstáculos pueden ocupar:

- Una cuadrícula,
- 4 cuadrículas en forma de T,
- 4 cuadrículas en forma de L.

### 2.5 Estados

#### 2.5.1 Positivos
- **Regeneración:** Este estado regenera la vida de la criatura a razón de 5 vida/s durante 10 segundos.
- **Vuelo:** Las criaturas en vuelo reciben 5 % menos de daño y evitan los obstáculos generados en los biomas. Las criaturas que vuelan son inmunes a los efectos negativos del suelo.
- **Protección:** Las criaturas reciben un 40 % menos de daño.

#### 2.5.2 Negativos
- **Veneno:** Las criaturas reciben un 33 % más de daño.
- **Sangrado:** Las criaturas afectadas reciben un 10 de daño/s durante 4 segundos.
- **Ralentización:** Las criaturas afectadas disminuyen su velocidad en 3 niveles (mínimo 1).
- **Quemando:** Las criaturas afectadas ven su ataque reducido en un 30 %.

### 2.6 Comportamiento de personajes
Durante la partida, las criaturas se comportan de manera independiente. El comportamiento de cada criatura depende principalmente de las partes animales de las que esté compuesto, ya que son estas las que determinan qué recursos deben recoger, qué habilidades usar y cómo. El orden de acción siempre sigue una secuencia descendente: cabeza, torso y, finalmente, base.

1. **Orden de recogida de recursos:** 
   - Todas las criaturas recogerán primero los recursos que necesite la cabeza. Una vez recogidos, el animal pasará a recoger los recursos necesarios para el torso, y luego para la base. 
   - Si alguna parte no especifica recursos necesarios, se pasa automáticamente a la siguiente.
   - Si una sola pieza requiere varios recursos, los recogerá en función de la proximidad, comenzando por el más cercano.

   **Ejemplo:** Una criatura que necesite recoger dos árboles para activar la habilidad de la cabeza y tres rocas para la de la base, actuará así:
   - Primero recoge los árboles. Cuando haya recogido dos, pasará al torso. 
   - Si el torso no necesita recursos, se salta a la base. La base necesita rocas, así que las recogerá y, cuando tenga las necesarias, pasará de nuevo a recoger árboles si los hay.

2. **Activación de habilidades:** 
   - Al igual que con los recursos, el orden de activación de habilidades se hace de arriba hacia abajo, comenzando por la cabeza y repitiéndose al llegar a la base.
   - Las habilidades “gastan” recursos, así que es necesario especificar cómo hacerlo.

   **Condiciones de uso:** Para que una habilidad se active, la criatura debe haber recogido la cantidad suficiente de materiales. Los materiales se “marcan” al usar cualquier habilidad. Si varias piezas usan los mismos materiales, se puede usar la misma unidad de material (por ejemplo, si una criatura tiene cinco árboles y necesita dos para una habilidad y tres para otra, el total de árboles utilizados y marcados será tres).

   - Cuando se reinicia el ciclo de activación de habilidades desde la cabeza tras pasar por la base, todos los materiales marcados se eliminan.
   - Si una habilidad no tiene suficientes materiales, se pasa automáticamente a la siguiente pieza.

### 2.7 Fases de la partida
 Una vez que se ha encontrado partida con un jugador esta se divide en tres fases diferentes. La primera de ellas es la fase preparatoria, que solo se jugará una vez por partida. Tras esta viene la fase de combate, donde se enfrentarán las criaturas de los jugadores en combate a muerte. Finalmente la fase de tienda en la que los jugadores administren sus tótems y biomas. El conjunto de fase de combate y fase de tienda es una ronda. Una partida completa consta de cinco rondas, aunque en la última no hay fase de tienda.

#### 2.7.1 Fase preparatoria
 Una vez que comienza la partida, cada jugador elige dos tótems de entre tres aleatorios. Una vez elegidos, los jugadores pueden mezclar las partes de los tótems a su gusto sin gasto de dinero, ya que para esta fase no hay forma de obtener dinero, en la pantalla de tienda. Tras esto el jugador podrá elegir dónde colocar las criaturas en el terreno. En esta primera ronda, solo se pueden colocar dos criaturas por jugador. La ronda preparatoria dura un minuto.

#### 2.7.2 Fase de combate
 En esta fase las criaturas de ambos jugadores combaten automáticamente hasta que to
dos los animales de uno de los jugadores caigan. Al terminar la fase, se le asigna cierta cantidad de oro a cada jugador, que se detalla más adelante. Además de dinero, uno de los jugadores recibe un punto de victoria. La fase terminará cuando uno de los jugadores pierda todas las criaturas o tras una duración máxima de 1 min, momento en el cual se asigna un punto de victoria al jugador que más vida acumulada entre todas las criaturas tenga. Si se da el caso de que ambos jugadores tienen la misma vida, se considerará un empate. 

Como la partida dura cinco rondas, un jugador puede ganar al conseguir tres puntos de victoria o más, momento en el que acaba la partida. Si los jugadores tienen el mismo número de puntos de victoria al terminar la partida, está se considerará que ha terminado en empate.

#### 2.7.3 Fase de compra
 Con el dinero obtenido en la fase anterior el jugador puede realizar ciertas acciones para preparar la siguiente ronda. 
 
 En primer lugar, cada jugador puede comprar y vender partes de tótems. Las partes compradas no se utilizan automáticamente, para ello primero el jugador debe pagar el precio de deshacer tótem. Una vez hecho esto se pueden vender y cambiar las piezas del tótem. Las piezas en propiedad pero que no se estén utilizando se guardan en un almacén, con una capacidad máxima de 12 objetos, entre piezas y biomas. En segundo lugar, se pueden comprar biomas. Los biomas se compran y se guardan en el almacén para ser luego utilizados en la fase de combate. 
 
 Tanto las partes como los biomas son generados aleatoriamente, por lo que el jugador puede pagar para regenerar los artículos de la tienda y generar nuevos objetos. La tienda siempre tiene cuatro objetos aleatorios, y siempre aparecerá al menos un bioma y una parte de tótem. Finalmente, los jugadores pueden utilizar dinero para ampliar el número máximo de tótems. Al comienzo de la partida la cantidad máxima es de 2 tótems, pero podrá ampliarse hasta 5. Este aumento de tótems para colocar se hace con la compra de experiencia (XP), que aumentará el nivel del jugador. 
 
 Antes de comenzar la batalla los jugadores deben colocar los biomas y tótems que poseen. En total esta fase dura 2 minutos.

### 2.8 Economía
Entre rondas, los jugadores podrán hacer uso de la divisa obtenida durante la partida para organizar su estrategia.
#### 2.8.1 Dinero obtenido
 En primer lugar se debe tratar el modo y cantidad de obtener dinero durante la partida. 
 
 Al terminar cada una de las rondas, independientemente del resultado, los jugadores son recompensados con una cantidad de dinero que se irá incrementando con el paso de las rondas. Ambos reciben la misma cantidad base.
 
 Para calcular este dinero base otorgado en cada ronda, se utiliza la siguiente fórmula ajustable que permite alterar los valores para balancear la economía de juego de una forma más sencilla

- **Fórmula de recompensa:**  
   - `a = Recompensa base fija por cada ronda`  
   - `b = Incremento progresivo`  
   - `c = Exponente de crecimiento por ronda`  
  
   Esta es una fórmula lineal, mucho más apta para una progresión constante y controlada, manteniendo los precios en la tienda relativamente estables.
   
   **Ejemplo:** Con los valores `a = 20`, `b = 5` y `c = 1.5`, el dinero obtenido se ajusta para una progresión constante.

#### 2.8.2 Gastos
- **Reroll:**  En primer lugar tenemos la compra de objetos que incluyen partes de animales y biomas. Al jugador se le presentan 4 objetos a comprar aleatorios, asegurando que hay mínimo una pieza y un bioma disponible.
 El jugador puede gastar una cantidad de dinero para cambiar los objetos de forma aleatoria. Esta acción cuesta dinero y se incrementa con cada uso, volviendo a su valor original en la siguiente ronda. El valor de regeneración de tienda inicial debe ser proporcional al dinero otorgado en la primera ronda, de manera que se limite su uso al principio, pero que según avance la partida sea más común poder usarlo.
   - **Fórmula de reroll:**  
      - `a = Recompensa base fija`  
      - `D = Divisor para coste inicial`  
      - `d = Divisor asociado al incremento gradual`  
   - **Ejemplo:** Con `a = 20`, `D = 5` y `d = 8`, se ajusta el valor del reroll para el primer turno.

- **Piezas:**  En primer lugar, las piezas de torso y base tendrán el mismo precio mientras que las cabezas tendrán un precio más bajo, ya que es una pieza más débil. Además, el precio de todas las partes juntas debe ser igual a un tercio de la recompensa inicial.
   - **Fórmula del precio de las piezas:**  
      - `C = Precio de Cabeza`  
      - `a = Recompensa base`  
      - `T = Precio de torso y patas`  

- **Rehacer:**  Para finalizar con los tótems, como ya se ha comentado, habrá que pagar un precio para poder rehacer los tótems. Este precio comenzará bajo pero irá aumentando según avance la partida.
   - **Fórmula de rehacer tótem:**  
      - `ronda = Nº de ronda`  
      - `m = Multiplicador de ronda`  

- **Biomas:** A continuación se tratará el precio de los biomas. Estos tendrán un precio base que aumentará en relación al número de biomas de ese tipo que hay en uso y el número de ronda en el que se encuentre la partida:
    - **Fórmula del precio de los biomas:**
        - `Preciobioma= precio de cada bioma en la tienda`
        - `a = Recompensa base fija por cada ronda utilizada en la fórmula de Recompensa`
        - `R = Número de rondas transcurridas`
        - `g = Incremento porcentual por ronda`
        - `B = Número de biomas activos del mismo tipo`
        - `i = Incremento porcentual de cada bioma`
 - **Ejemplo:** Con `a = 20`, `g = 1` y `i = 2`, los valores de compra de biomas serían:

- **Experiencia:**  Finalmente, el último artículo disponible en la tienda es el de la XP, que te permite subir de nivel y poder colocar más tótems en el tablero.
    - **Fórmula del precio de los biomas:**
        - `a = Recompensa base fija por cada ronda utilizada en la fórmula de Recompensa`
        - `N = Nivel actual del jugador`
        - `h = Exponente para aumentar`
    - **Ejemplo:** Con `a = 20`, `g = 1` y `i = 2`, los valores de compra de biomas serían:

- **Venta:**   Además de comprar piezas, se pueden vender todas aquellas piezas que se posean, siempre que estén en el inventario y no en un tótem. El precio de venta será siempre el mismo para cada tipo de pieza, el cual será un 50% del valor original.

### 2.9 Ligas
 A medida que el jugador juega partidas, obtiene una serie de puntos clasificatorios que le situarán en una liga en función de su nivel. Al comenzar, todos los jugadores empiezan con 0 puntos clasificatorios, y en la liga de bronce. A medida que los jugadores vayan obteniendo puntos al ganar partidas, irán subiendo de ligas, pero del mismo modo, pueden bajar.
 
 Estas ligas son:
- Bronce
- Plata
- Oro
- Esmeralda
- Amatista
- Jade
- Diamante
- Ónix
- Chamán
- Avatar

 Las ligas se agrupan según un conjunto de nivel medio, de modo que Bronce, Plata y Oro son las Ligas Tribales; Esmeralda, Amatista y Ónix las Ligas Maestras; Jade, Diamante y Chamán, las ligas Supremas; y Avatar sería la Liga Mítica, donde el jugador ya no puede aumentar de liga, pero puede conseguir más puntos para ser el mejor jugador de Doodem. 
 
 Estos conjuntos de ligas tienen influencia a la hora del sistema de puntos, de modo que la cantidad de puntos que se ganan y pierden en las ligas tribales es menor que en las otras ligas. Este sistema hace que las partidas sean más duras a medida que subes de rango, haciendo más duro perder, pero más satisfactorio ganar.
 
 El sistema de puntuación al ganar una partida es utiliza la siguiente fórmula para calcular los puntos ganados o perdidos:

- K = factor de ajuste, que determina la máxima cantidad de puntos que se pueden
 ganar o perder en una partida.
- D = diferencia de puntos entre ambos jugadores, definida como PA- PB.
  
De este modo, el factor de ajuste k según las ligas es:
- Ligas tribales: k = 20
- Ligas maestras: k = 30
- Ligas supremas: k = 40
- Liga mítica: k = 50

 Finalmente, la cantidad de puntos necesarios para alcanzar cada liga son:

### 2.10 Controles
*Doodem* se juega principalmente en navegadores y está diseñado para PC y dispositivos móviles. Los controles son sencillos para que puedan adaptarse a diferentes dispositivos:

- **PC:** Uso del ratón para seleccionar y colocar tótems y biomas en el tablero.
- **Móvil y tablet:** Controles táctiles para seleccionar y arrastrar los elementos en el tablero.
  
Los controles básicos permiten al jugador interactuar de manera intuitiva con el entorno y las criaturas, haciendo más accesible el flujo de juego en múltiples plataformas.

### 2.11 Ejemplo de partida
A continuación, se presenta un ejemplo de cómo se desarrolla una partida de *Doodem*, describiendo las fases y las estrategias que un jugador podría usar para competir eficazmente.

1. **Fase preparatoria:**  
   El jugador elige dos tótems iniciales y combina sus partes para crear criaturas con habilidades estratégicas.
   
2. **Fase de combate:**  
   Las criaturas generadas se enfrentan automáticamente a las del jugador rival hasta que uno de los equipos sea derrotado. Al finalizar, ambos jugadores reciben oro en función del rendimiento.

3. **Fase de compra:**  
   Con el oro obtenido, el jugador puede comprar nuevas partes de tótems o biomas en la tienda, ajustar su estrategia de combate y preparar su equipo para la siguiente ronda.
   
Este proceso se repite durante cinco rondas, con la posibilidad de ganar la partida al acumular tres puntos de victoria. Los jugadores deben adaptar sus estrategias, combinando criaturas y personalizando el terreno de juego para maximizar su ventaja competitiva.

---

## 3. Arte

### 3.1 Estética general
La estética de *Doodem* se inspira en un estilo visual que combina elementos de la naturaleza con un toque caricaturesco. El objetivo es que los jugadores sientan que están en un ambiente salvaje y dinámico, donde cada criatura y bioma posee características únicas que refuerzan la identidad visual del juego.

El juego cuenta con una estética cartoon y colorida, ya que se ha tomado inspiración de los tótems nativos americanos, aunque llevándolo todo a un territorio nacional. Esta estética permite atraer también a un público más infantil, que se sentirá atraído tanto por la mecánica de combinar los animales como por la propia estética del juego.

En cuanto al apartado visual del juego, este combina tanto modelos en 3D como escenarios en 2D creados a mano. Además, las interfaces se han creado también en 2D y usando la misma paleta de colores del juego.


### 3.2 Arte conceptual
El arte conceptual se centra en el diseño de las criaturas y biomas, asegurando que cada tótem transmita la esencia del animal que representa. Los diseños de los tótems son sencillos pero icónicos, permitiendo que los jugadores los identifiquen fácilmente en el tablero.

- **Tótems de animales reales:** Cada tótem representa un animal característico de la fauna ibérica, con detalles visuales que destacan sus rasgos distintivos.
- **Biomas personalizados:** Los biomas incluyen bosques, montañas y áreas áridas, cada uno diseñado con elementos visuales que indican sus efectos en el juego.

Por un lado se creará el arte conceptual de los tótems, que da una representación desde distintos puntos de vista de las distintas facciones del animal presentes en el tótem y que luego servirá para modelar el tótem en 3D.
 Además, también se usará el mismo programa para la creación de los sprites que formarán los escenarios en los que se disputarán las batallas. 
Por último, también se crearán arte conceptual de cómo se verían los animales mezclados una vez se han invocado mezclando las piezas que los jugadores elijan de los tótems.

### 3.3 Arte 3D
El juego utiliza modelos 3D para los tótems y el entorno, brindando una experiencia visual envolvente. La prioridad es que los modelos sean reconocibles y se integren con la estética general del juego.

- **Tótems en 3D:** Los tótems están diseñados en tres dimensiones, permitiendo que se vean desde múltiples ángulos y manteniendo su carácter caricaturesco.
- **Escenarios en 3D:** El terreno está dividido en casillas, lo cual permite a los jugadores posicionar sus criaturas y biomas de manera estratégica. Los modelos de los biomas son también tridimensionales, con texturas y detalles que los diferencian visualmente y ayudan a crear un entorno coherente.

Para crear los tótems primero se usará un forma de tótem base dividida en tres partes a la que luego se le añadirán las facciones típicas de cada animal. De este modo, el flujo de creación de un nuevo tótem consiste en crear las partes nuevas del animal en vez de tener que crear el tótem desde 0, lo que ahorra una gran cantidad de tiempo.

Cabe destacar que como el juego debe funcionar en navegador, los modelos no pueden ser high poly, ya que podría provocar fallos en el rendimiento del juego. Debido a esto se optará por realizar modelos low poly y añadirles los detalles en forma de textura. De este modo se consigue una amplia cantidad de detalles sin necesidad de polígonos y además contribuye a mejorar la impresión de que los tótems están tallados en madera debido a la rudeza de los ángulos.

En cuanto al texturizado de los mismos, los detalles se han pintado a mano sobre el unwrapping que se elabora en Blender, esto nos permite añadir detalles como plumas, pupilas o incluso partes del cuerpo sin tener que recurrir al modelado de nuevas partes. Además, se ha usado tanto una textura de madera como el mapa de normales de las propias texturas pintadas a mano para dar al tótem el aspecto de estar hecho de madera y de tener ciertas partes talladas a mano.

---

## Capítulo 4: Sonido y Música

### 4.1 Música
Doodem contará con varios temas musicales que se utilizarán en diferentes ocasiones. Todos los temas están compuestos con instrumentos típicos de las culturas americanas nativas, evocando a la naturaleza y la guerra. Partiendo de un tema principal, se abordarán otros tres temas ambiente para las batallas y uno para el ambiente durante la navegación del menú fuera de las batallas.

El tema principal debe ser el más reconocible, capaz de identificar al juego. Este tema será utilizado en productos promocionales como tráilers. Además de eso sonará al iniciar el juego a modo de presentación, y tras acabar, dejará paso al tema de ambiente.

Este tema debe ser tranquilo y funcionar como acompañamiento a las interacciones del jugador en el menú para evitar el silencio. Aunque sigue inspirado en músicas nativas su enfoque será mucho menos agresivo.

Finalmente, cada uno de los tres temas de batalla está inspirado en cada uno de los biomas no negativos, bosque, lago y montaña. Cuando la partida comienza, se selecciona un bioma aleatorio, momento en el cual empezará a sonar el tema asociado a él.

### 4.2 Efectos de sonido
En lo relativo a los sonidos se tomarán sonidos naturales y fuertes que simbolizan la naturaleza competitiva del juego y su mecánica central de animales.

---

## Capítulo 5: Interfaces y menús

### 5.1 Flow-Charts
Se utilizan diagramas de flujo (_flow-charts_) para organizar el flujo de interfaces dentro del juego y facilitar la navegación del usuario. Estos diagramas ayudan a visualizar la secuencia de pantallas, menús y opciones que el jugador verá y usará durante su experiencia de juego, permitiendo una estructura clara y fácil de entender.

### 5.2 Mockups
Se presentan _mockups_ o maquetas visuales para representar el diseño de las principales interfaces del juego. Estos mockups muestran cómo se verá cada pantalla, incluyendo la disposición de elementos como botones, menús y otros componentes interactivos. 

Cada pantalla está diseñada para optimizar la navegación y asegurar que el jugador pueda acceder fácilmente a las diferentes secciones del juego, tanto en la interfaz principal como en las secciones específicas de combate, tienda y configuración del juego.

---

## Capítulo 6: Monetización y modelo de negocio

### 6.1 Modelo de negocio
El modelo de negocio de _Doodem_ se basará en un enfoque de fidelización. El juego será lanzado de forma gratuita, pero con opciones de expansión que se pueden adquirir. A lo largo del tiempo, se lanzarán expansiones de pago que no afectarán el balance del juego ni darán ventaja competitiva a los que las obtengan. Estas expansiones buscan mantener a los jugadores actuales dentro del juego y atraer a nuevos posibles jugadores.

#### 6.1.1 Información del usuario
- **¿Quién es?** Jugador casual de cualquier edad y procedencia.
- **¿Situación?** El juego va dirigido a usuarios principalmente con poco tiempo para jugar, y a aquellos que tengan más tiempo libre para profundizar y dedicar tiempo. Económicamente el jugador no tiene una gran capacidad de realizar gastos.
- **Cómo es**: El jugador es competitivo, estratega y social.
- **¿Qué quiere?** Quiere matar el tiempo, pasar el rato, interactuar, competir y pensar.
- **¿Aficiones?** Juegos casuales, juegos de estrategia y de competición. Animales.
- **¿Qué necesita?** Entretenimiento rápido, capacidad de desarrollo mental y capacidad de desarrollo estratégico satisfactorio tanto si se quiere profundizar como si no. Capacidad de socialización.
- **¿Actividad?** Actividad habitual, aunque puedan ser sesiones cortas según el usuario. Una persona más competitiva o enfocada en la estrategia puede dedicar más tiempo.

#### 6.1.2 Mapa de empatía
El mapa de empatía está diseñado para comprender mejor a los usuarios del juego. Incluye preguntas y factores clave que permiten conocer los deseos, necesidades y comportamientos del jugador. Esta herramienta es crucial para desarrollar un enfoque de monetización efectivo y una experiencia de usuario atractiva.

#### 6.1.3 Caja de herramientas
La caja de herramientas incluye las estrategias y tácticas que el equipo de desarrollo empleará para mantener y mejorar la experiencia del jugador. Estas herramientas están alineadas con los objetivos del modelo de negocio y buscan promover la retención de jugadores, la adquisición de nuevos usuarios y el incremento en la participación dentro del juego.

#### 6.1.4 Modelo de lienzo
El modelo de lienzo es un esquema que representa los componentes principales del modelo de negocio, incluyendo el valor del juego para los usuarios, las fuentes de ingresos, la estructura de costos y las alianzas clave necesarias para el éxito del juego en el mercado.

### 6.2 Monetización
El sistema de monetización en _Doodem_ es de tipo freemium, permitiendo jugar gratis con opción a compras adicionales para mejorar la experiencia. El juego contará con actualizaciones periódicas de contenido, aproximadamente cada dos meses. Estas actualizaciones incluirán nuevos personajes jugables y avatares para los jugadores. Parte del contenido será gratuito y otra parte estará disponible para compra, tanto con dinero real como con moneda virtual.

La moneda virtual se puede obtener de distintas maneras: jugando partidas (obteniendo una cantidad limitada de esta forma), comprándola con dinero real en la tienda virtual o viendo videos con anuncios. Con esta moneda, los jugadores pueden adquirir tanto las expansiones como skins para los personajes, aunque estas últimas solo podrán obtenerse con la moneda virtual, no con dinero real.

En el momento del lanzamiento, la única forma de monetización serán las monedas virtuales para conseguir skins. Sin embargo, se planean eventos especiales y promociones durante festividades como Navidad, Pascua o Halloween, ofreciendo ofertas especiales y packs de contenido. Estos eventos también incluirán cambios en el aspecto visual del juego para mantener el interés de los jugadores.

Finalmente, el juego contará con torneos para los jugadores más competitivos, lo cual fomenta la mejora continua de los jugadores y da visibilidad al juego, atrayendo nuevos jugadores al ecosistema competitivo.

#### 6.2.1 Tienda
La tienda de _Doodem_ estará dividida en tres secciones:

1. **Tienda de expansiones**: Donde se podrán comprar paquetes de expansiones que incluyen nuevos tótems y escenarios para usar en las partidas.
2. **Tienda de cosméticos o skins**: Donde se podrán comprar cambios de color y otros elementos estéticos para los tótems que ya tiene el jugador.
3. **Visualización de anuncios**: El jugador podrá ver un breve anuncio para ganar una pequeña cantidad de monedas.

La moneda que se usa en la tienda es distinta de la utilizada durante las partidas y puede obtenerse jugando o comprando con dinero real. Para comprar monedas, el jugador es dirigido a la pantalla de pago de su banco para confirmar la transacción.

Todos los objetos de la tienda se compran con la misma moneda, y la tienda cambia cada día, añadiendo ofertas especiales en fechas relacionadas con festividades.
