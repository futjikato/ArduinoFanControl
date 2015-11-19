# Proxy for serial connection to arduino

Works with the following arduino code.  
I tried to comment it as well as possible but it is still not as stable and genric as it could be.

```c
void setup() {
  pinMode(3, OUTPUT);
  // ... set all pins
  
  // you can change baudrate if you will. app starts with a setup screen
  Serial.begin(9600);
}

void loop() {
  // protocol defines requests always in 3 bytes
  if (Serial.available() > 2) {
    // read the incoming bytes
    int type = Serial.read();
    int port = Serial.read();
    int spower = Serial.read();

    if (type == 1) {
      Serial.write(3);
      // ... write each pin that is connected out here
      
      // 0 byte as end marker
      Serial.write(0);
    } else if(type == 2) {
      // check if port is valid pin
      if (port == 3) { 
        // set pwm freq
        analogWrite(port, spower);
        
        Serial.write(1); // first answer byte is status 0||1
        Serial.write(0); // 0 byte as end marker
        return;
      }
      Serial.write(0); // first answer byte is status 0||1
      Serial.write(0); // 0 byte as end marker 
    } else {
      Serial.write(0); // first answer byte is status 0||1
      Serial.write(0); // 0 byte as end marker
    }
  }
}
```