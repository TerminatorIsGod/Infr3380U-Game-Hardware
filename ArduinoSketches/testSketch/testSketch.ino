#include<Wire.h>
const int MPU=0x68; 
int16_t AcX,AcY,AcZ,Tmp,GyX,GyY,GyZ;

//Button Stuff
int buttonState = 0;

// Rotary Encoder Inputs
#define CLK 2
#define DT 3

int counter = 0;
int currentStateCLK;
int lastStateCLK;
String currentDir ="";



void setup() {
  // put your setup code here, to run once:
  
  pinMode(2, INPUT);

  // Set encoder pins as inputs
	// pinMode(CLK,INPUT);
	// pinMode(DT,INPUT);
  // lastStateCLK = digitalRead(CLK);


  Serial.begin(9600);

  
}

void loop() {
  
  buttonState = digitalRead(2);
  // if (buttonState == HIGH)
  //   Serial.println("PRESSED!");

  Serial.println(buttonState);
  delay(1);

}

void RotaryEncoderInput()
{
	// Read the current state of CLK
	currentStateCLK = digitalRead(CLK);

	// If last and current state of CLK are different, then pulse occurred
	// React to only 1 state change to avoid double count
	if (currentStateCLK != lastStateCLK  && currentStateCLK == 1){

		// If the DT state is different than the CLK state then
		// the encoder is rotating CCW so decrement
		if (digitalRead(DT) != currentStateCLK) {
			counter --;
			currentDir ="CCW";
		} else {
			// Encoder is rotating CW so increment
			counter ++;
			currentDir ="CW";
		}

		Serial.print("Direction: ");
		Serial.print(currentDir);
		Serial.print(" | Counter: ");
		Serial.println(counter);
	}

	// Remember last CLK state
	lastStateCLK = currentStateCLK;
}

void OutputToArduino()
{
    switch(Serial.read())
  {
    case 'A':
      Serial.println("Recieved 'A' on Arduino!");
      break;
  }

}
