# Ricochet Ball
---
The purpose of this program is to have a better understanding of how refresh
rate and motion work, relationship among three quantities: ball speed (pix/sec),
ball speed (pix/tic), and animation clock speed (tic/sec), bounce a ball of a
hard flat surface (wall), and convert traditional angles into computer graphic
angles.

## Specifications
---
* When a user clicks on "New" button, all the input fields are set to blanks and
a small red ball will appear in the center of the graphical area.
* In the speed box, the user enters the motion speed of the ball. This is called
the "linear speed" of the animated object, measured in pix/sec.
* In the direction box, the user enters a number in degrees. The ball will
initially travel in the direction of the inputted degrees. This should be in
type double.
* When the user clicks on the start button, the two input numbers are read: speed
and direction. By using those two numbers, the ball starts moving in the given
direction. ***SPEED AND DIRECTION CANNOT BE NEITHER NULL NOR NEGATIVE NUMBERS***
* When the ball reaches one of the four walls (four sides of the rectangle), it
bounces off the wall and continues. Then the ball continues in a new direction
until the user clicks "Quit" or "New".
* If the user clicks "Quit", then the frame closes.
* if the user clicks "New", then the ball stops and all the input fields become
blank.
* While the ball is moving, the coordinates of the ball are displayed in the X
and Y output fields.

### Math requirements
- Convert the input degrees to radians (&theta).
- Let ***S*** be the linear speed of the ball in pix/sec, which is the input from
the user. Let ***M*** be the rate of the motion clock in tic/sec, which is set
by the programmer.
- ```Speed of the ball = C = S/M```
- Compute \delta x and \delta y using the right triangle trigonometry:
    \delta x = C * cos(\theta)
    \delta y = C * sin(\theta)
- Each time the motion clock tics, the amount of \delta x is added to the x-
coordinate of the ball, and \delta y is added to the y-coordinate of the ball.

## Prerequisites
---
* A virtual machine
* Install mcs

## Instruction on how to run the program
---
1. chmod +x build.sh then ./build.sh
2. sh build.sh

Copyright [2019] [Kien Truong]
