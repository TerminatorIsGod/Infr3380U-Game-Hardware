unsigned long lastTime = 0;
int buttonState = 0;
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(2, INPUT);
}

void loop() {

  buttonState = digitalRead(2);
  if(buttonState == HIGH)
  {
    Serial.println("BUTTON PRESSED");  
  }
  // put your main code here, to run repeatedly:
  if(millis() > lastTime + 5000)
  {
    Serial.println("ree");
    lastTime = millis();
  }

   
  switch(Serial.read())
  {
    case 'A':
      Serial.println("Recieved 'A' on Arduino!");
      break;
  }
}
