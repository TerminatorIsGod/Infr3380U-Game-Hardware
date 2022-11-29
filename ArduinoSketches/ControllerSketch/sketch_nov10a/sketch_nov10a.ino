
#include<Wire.h>
#include <Encoder.h>

//Accelorometer + Gyro
const int MPU_addr = 0x68; // I2C address of the MPU-6050
int16_t AcX, AcY, AcZ, Tmp, GyX, GyY, GyZ;
double pitch,roll;

//Button Stuff
int buttonState = 0;

// Rotary Encoder Inputs
Encoder myEnc(3, 4);
long oldPosition  = -999;

void setup() {

  //Button Input
  pinMode(2, INPUT_PULLUP);
  digitalWrite(2, HIGH);


  //Vibration Output
  pinMode(8, OUTPUT);




  Wire.begin();
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x6B);  // PWR_MGMT_1 register
  Wire.write(0);     // set to zero (wakes up the MPU-6050)
  Wire.endTransmission(true);
  Serial.begin(9600);
}

void loop() {

  
  //Button
  buttonState = digitalRead(2);
  if(buttonState == HIGH)
  {
    Serial.println("Pressed");
  }
  else
  {
    Serial.println("Released");
  }

  //Rotary Encoder
  long newPosition = myEnc.read();
  if (newPosition > oldPosition) {
    oldPosition = newPosition;
    Serial.println("CounterClockwise");
  }
  else if (newPosition < oldPosition) {
    oldPosition = newPosition;
    Serial.println("Clockwise");
  }



  //Accelorometer + Gyroscope
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x3B);  // starting with register 0x3B (ACCEL_XOUT_H)
  Wire.endTransmission(false);
  Wire.requestFrom(MPU_addr, 14, true); // request a total of 14 registers
  int t = Wire.read();
  AcX = (t << 8) | Wire.read(); // 0x3B (ACCEL_XOUT_H) & 0x3C (ACCEL_XOUT_L)
  t = Wire.read();
  AcY = (t << 8) | Wire.read(); // 0x3D (ACCEL_YOUT_H) & 0x3E (ACCEL_YOUT_L)
  t = Wire.read();
  AcZ = (t << 8) | Wire.read(); // 0x3F (ACCEL_ZOUT_H) & 0x40 (ACCEL_ZOUT_L)
  t = Wire.read();
  Tmp = (t << 8) | Wire.read(); // 0x41 (TEMP_OUT_H) & 0x42 (TEMP_OUT_L)
  t = Wire.read();
  GyX = (t << 8) | Wire.read(); // 0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)
  t = Wire.read();
  GyY = (t << 8) | Wire.read(); // 0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)
  t = Wire.read();
  GyZ = (t << 8) | Wire.read(); // 0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)
  t = Wire.read();

  //get pitch/roll
  getAngle(AcX,AcY,AcZ);

  //send the data out the serial port
 
  Serial.print("X "); Serial.println(pitch);
  Serial.print("Y "); Serial.println(roll);

  // Serial.print("AcX = "); Serial.print(AcX);
  // Serial.print(" | AcY = "); Serial.print(AcY);
  // Serial.print(" | AcZ = "); Serial.println(AcZ);
  // Serial.print(" | Tmp = "); Serial.print(Tmp / 340.00 + 36.53); //equation for temperature in degrees C from datasheet
  // Serial.print(" | GyX = "); Serial.print(GyX);
  // Serial.print(" | GyY = "); Serial.print(GyY);
  // Serial.print(" | GyZ = "); Serial.println(GyZ);

  //Vibration Motor
  switch(Serial.read())
  {
    case 'V':
      digitalWrite(8, HIGH);
      break;
    case 'I':
      digitalWrite(8, 0);
      break;
  }

  delay(20);
}

//convert the accel data to pitch/roll
void getAngle(int Vx,int Vy,int Vz) {
double x = Vx;
double y = Vy;
double z = Vz;

pitch = atan(x/sqrt((y*y) + (z*z)));
roll = atan(y/sqrt((x*x) + (z*z)));
//convert radians into degrees
pitch = pitch * (180.0/3.14);
roll = roll * (180.0/3.14) ;
}


