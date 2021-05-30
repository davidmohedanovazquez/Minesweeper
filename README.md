# Minesweeper Game
El conocido juego del buscaminas, implementado en C# y utilizando Windows Forms, en Visual Studio.

## Características
El juego consta con tres niveles distintos: Fácil, Intermedio y Difícil en los que varían la cantidad de casillas y la cantidad de minas.
También lleva un estudio de las estadísticas de las partidas anteriores, cuyos datos se almacenan en un fichero (`statistics.txt`).

## Cómo lanzarlo
Clonar este repositorio y abrir el proyecto en Visual Studio, pinchando directamente sobre `MinesweeperGame.sln`. Antes de ejecutar el programa es importante cambiar la ruta del fichero `statistics.txt` por la del ordenador desde el que se ejecuta, tanto en  la línea 12 del fichero `Minesweeper.cs` como en la línea 14 del fichero `Statistics.cs`.
Ejecutar y disfrutar del juego.

## Cómo jugar
Los controles del juego son exactamente los mismos que los del juego original:
- Click izquierdo sobre una casilla para abrirla y descubrir su contenido.
- Un click derecho sobre una casilla para marcarla con una bandera como Bomba.
- Segundo click derecho sobre una casilla para marcarla como duda con una interrogación (?).
- Tercer click derecho sobre una casilla para volver a su estado inicial.
