    #include P18F4550.inc

    config  FOSC = HS		
    config  CPUDIV = OSC1_PLL2
    config  PLLDIV = 1 
    config  PWRT = OFF
    config  BOR = OFF
    config  WDT = OFF
    config  MCLRE = ON
    config  LVP = OFF
    config  ICPRT = OFF
    config  XINST = OFF
    config  DEBUG = OFF
    config  FCMEN = OFF
    config  IESO = OFF
    config  LPT1OSC = OFF
    config  CCP2MX = ON
    config  PBADEN = OFF
    config  USBDIV = 2
    config  VREGEN = OFF



    org 0x0000
    BRA START
    org 0x0008
    RCALL HISR
    BRA START
    org 0x0018
    RCALL LISR
    RETFIE
    

CONTROL	equ 0x01
COIN	equ 0x02

    
START
    MOVLW 0x0F
    MOVWF ADCON1, ACCESS
    MOVLW 0x07
    MOVWF CMCON, ACCESS
    
    CLRF TRISD, ACCESS
    MOVLW 0xF0
    MOVWF TRISB, ACCESS
    
    CLRF CONTROL, ACCESS
    CLRF COIN, ACCESS
    
    ;SET ALL MOTORS TO STOP (ALL 1)
    MOVWF 0xF0
    MOVWF LATB, ACCESS
    SETF LATD, ACCESS
 
    
   ; ENABLE THE PRIORITY INTERRUPT
    BSF RCON, IPEN, ACCESS
    
    ; INT0
    BSF INTCON2, INTEDG0, ACCESS
    BCF INTCON, INT0IF, ACCESS
    BSF INTCON, INT0IE, ACCESS
    
    ; INT1
    BSF INTCON2, INTEDG1, ACCESS
    BCF INTCON3, INT1IP, ACCESS
    BCF INTCON3, INT1IF, ACCESS
    BSF INTCON3, INT1IE, ACCESS
    
    ; ENABLE LOW AND HIGH PRIORITY GLOBAL INTERRUPT
    BSF INTCON, GIEH, ACCESS
    BSF INTCON, GIEL, ACCESS

    ; ENABLE RX INTERRUPT
    BSF IPR1, RCIP, ACCESS
    BSF PIE1, RCIE, ACCESS
    
    ; 9600 BAUD
    ; SYNC = 0, BRG16 = 1, BRGH = 1
    MOVLW 0x02
    MOVWF SPBRGH, ACCESS
    MOVLW 0x08
    MOVWF SPBRG, ACCESS
    
    ; BAUDCON: 00001000
    MOVLW 0x08
    MOVWF BAUDCON, ACCESS
    
    ; ENABLE RX
    ; RCSTA: 10010000
    MOVLW 0x90
    MOVWF RCSTA, ACCESS
    
    
MAIN
CHECK1
    MOVLW 0x01
    CPFSEQ  CONTROL, ACCESS
    BRA CHECK2
    BRA MOTOR1
CHECK2
    MOVLW 0x02
    CPFSEQ  CONTROL, ACCESS
    BRA CHECK3
    BRA MOTOR2
CHECK3
    MOVLW 0x03
    CPFSEQ  CONTROL, ACCESS
    BRA CHECK4
    BRA MOTOR3
CHECK4
    MOVLW 0x04
    CPFSEQ  CONTROL, ACCESS
    BRA CHECK5
    BRA MOTOR4
CHECK5
    MOVLW 0x05
    CPFSEQ  CONTROL, ACCESS
    BRA CHECK6
    BRA MOTOR5
CHECK6
    MOVLW 0x06
    CPFSEQ  CONTROL, ACCESS
    BRA CHECK7
    BRA MOTOR6
CHECK7
    MOVLW 0x07
    CPFSEQ  CONTROL, ACCESS
    BRA CHECK8
    BRA MOTOR7
CHECK8
    MOVLW 0x08
    CPFSEQ  CONTROL, ACCESS
    BRA CHECK9
    BRA MOTOR8
CHECK9
    MOVLW 0x09
    CPFSEQ  CONTROL, ACCESS
    BRA CHECKA
    BRA MOTOR9
CHECKA
    MOVLW 0x0A
    CPFSEQ  CONTROL, ACCESS
    BRA CHECKB
    BRA MOTORA
CHECKB
    MOVLW 0x0B
    CPFSEQ  CONTROL, ACCESS
    BRA CHECKC
    BRA MOTORB
CHECKC
    MOVLW 0x0C
    CPFSEQ  CONTROL, ACCESS
    BRA MAIN
    BRA MOTORC
MOTOR1
    BCF LATD, 0, ACCESS
MOTOR2
    BCF LATD, 1, ACCESS
MOTOR3
    BCF LATD, 2, ACCESS
MOTOR4
    BCF LATD, 3, ACCESS
MOTOR5
    BCF LATD, 4, ACCESS
MOTOR6
    BCF LATD, 5, ACCESS
MOTOR7
    BCF LATD, 6, ACCESS
MOTOR8
    BCF LATD, 7, ACCESS
MOTOR9
    BCF LATB, 4, ACCESS
MOTORA
    BCF LATB, 5, ACCESS
MOTORB
    BCF LATB, 6, ACCESS
MOTORC
    BCF LATB, 7, ACCESS
    
HISR    
    ;DETECTS INT 0 EVENTS
    SETF LATD, ACCESS
    MOVLW 0xF0
    MOVWF LATB, ACCESS
    BCF INTCON3, INT1IF, ACCESS
    RETURN
    
LISR
    ; DETECTS INT1 EVENTS
    INCF COIN, F, ACCESS
    BCF INTCON3, INT1IF, ACCESS
    RETURN
    
RX_ISR
    BTFSC RCSTA, FERR, ACCESS
    BRA RX_ERR
RX_OERR
    BTFSS RCSTA, OERR, ACCESS
    BRA RX_NO_ERR
RX_ERR
    BCF RCSTA, CREN, ACCESS
    BSF RCSTA, CREN, ACCESS
    RETFIE FAST
RX_NO_ERR
    MOVF RCREG, W, ACCESS
    MOVWF CONTROL, ACCESS
    RETFIE FAST    

    end

