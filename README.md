# csismartcard
Minimum mimicking for javax.smartcardio
Sometimes we need to port code(s) from java to csharp and vice versa. This library tries to achieve that especially for PCSC smartcard operation. Some of methods are missing on this.

##  NuGet installation
see https://www.nuget.org/packages/csismartcard/2.0.4

###  API Example
A simple example of using the API is:
```csharp
// show the list of available terminals
TerminalFactory factory = TerminalFactory.getDefault();
List<CardTerminal> terminals = factory.terminals().list();
terminals.list().ForEach(o => Console.WriteLine(o.getName()));
Console.WriteLine();
// get the first terminal
CardTerminal terminal = terminals.getTerminal(0);
// establish a connection with the card
Card card = terminal.connect<Card>();
Console.Write("ATR: ");
card.getATR().getBytes().ForEach(b => Console.Write("{0:X2} ", b));
Console.WriteLine();
CommandAPDU command = new CommandAPDU(cl);
ResponseAPDU response = card.getBasicChannel().transmit(command);
// disconnect
card.disconnect(false);
```
