# Fractal Sandbox

## Currently supported Fractals
1. Buddhabrot
2. Glynn Set
3. Julia Set
4. Logistic Map
5. Mandlebrot Set
6. Worley Noise

## Using the API
This program is controlled via CLI using the following keywords:
### [fractal]
This keyword is substituted for the name of the fractal you would like to render:
1. buddhabrot
2. glynn
3. julia
4. mandelbrot
### load
* Optional keyword
* If you've previously rendered a fractal and saved its configuration using the "save" keyword you can redraw it without have to re-render it. 
### render
If you don't include this keyword, the fractal won't render.
### save 
* Optional keyword
* If you'd like to save the data of a rendered fractal so you can re-render it without having to draw it, use the save keyword.
* A settings.json, distance.dat and exposure.dat will be saved on your desktop along with the drawn image.
### draw
* Draws and saves the image to your desktop.
### /?
* Quick help menu if you don't feel like opening this readme. 
## Usage Examples
Render a Buddhabrot fractal, save the data and draw it to a file.
```
buddhabrot render save shade draw
```
Load a Mandelbrot fractal, skip rendering it, and draw it to a file.
```
mandelbrot load shade draw
```
