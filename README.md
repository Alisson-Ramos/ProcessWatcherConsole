# 🖥️ ProcessWatcherConsole

Um monitor de processos em **C# (.NET)** que exibe em tempo real no **console**:

- Nome do processo  
- ID (PID)  
- Usuário dono do processo  
- Tempo rodando  
- Evento: **Iniciado** ou **Encerrado**  

O monitor utiliza **WMI (Windows Management Instrumentation)** para capturar eventos de criação e encerramento de processos.

---

## 🚀 Funcionalidades

- Monitoramento em tempo real de processos
- Exibição de informações detalhadas:
  - Nome do processo
  - PID
  - Usuário que iniciou o processo
  - Tempo de execução
- Eventos de **start** e **stop** destacados no console

---

## 📦 Requisitos

- Windows (necessário WMI)
- [.NET 6 ou superior](https://dotnet.microsoft.com/download/dotnet)
- **Executar como Administrador** para capturar todos os processos
