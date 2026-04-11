Weather App


![status](https://img.shields.io/badge/status-active-brightgreen)
![platform](https://img.shields.io/badge/platform-windows-blue)
![.NET](https://img.shields.io/badge/.NET-10.0-purple)
![UI](https://img.shields.io/badge/UI-GunaUI2-orange)
![charts](https://img.shields.io/badge/charts-ScottPlot4-lightgrey)

Applicazione meteo per Windows sviluppata in C# (.NET 10, WinForms).  
Mostra meteo attuale, qualità dell’aria, previsioni, grafici storici e correlazioni tra temperatura e inquinamento.  
L’interfaccia è moderna grazie a GunaUI2 e lo sfondo cambia in base alle condizioni meteo.

---

📌 Funzionalità
- ricerca meteo per città  
- meteo attuale con icona dinamica  
- qualità dell’aria (AQI)  
- valori PM2.5 e PM10  
- previsioni meteo a 4 giorni  
- aggiornamento automatico dei dati  
- salvataggio e caricamento in JSON  
- sfondo dinamico basato sul meteo  
- grafici storici (temperatura, AQI, PM2.5, PM10)  
- grafico di correlazione temperatura–inquinamento  

---

🧩 Tecnologie utilizzate
- C# / .NET 10  
- WinForms  
- GunaUI2 (UI moderna)  
- ScottPlot 4.1.67 (grafici)  
- Newtonsoft.Json (gestione JSON)  

---

🌐 API utilizzate
L’app utilizza tre servizi di OpenWeather:

1. Current Weather API
Recupera:
- temperatura  
- icona meteo  

2. Forecast API
Usata per generare le previsioni dei prossimi 4 giorni.

3. Air Pollution API
Restituisce:
- AQI  
- PM2.5  
- PM10  

---

🔄 Aggiornamento automatico
L’app aggiorna i dati tramite un timer interno.  
Ad ogni aggiornamento:

- scarica i nuovi dati dalle API  
- aggiorna l’interfaccia  
- salva i valori nel file JSON locale  

---

💾 Gestione dei dati (JSON)
L’app salva automaticamente uno storico dei dati in un file JSON locale.

Contiene:
- temperatura  
- AQI  
- PM2.5  
- PM10  
- timestamp  

Lo storico viene usato per generare i grafici.

Se il file non esiste, viene creato automaticamente.

---

📊 Grafici (ScottPlot)
La finestra dedicata ai grafici include:

* Temperatura nel tempo
Andamento termico con asse temporale.

* AQI nel tempo
Evoluzione della qualità dell’aria.

* PM2.5 e PM10
Due curve sovrapposte con legenda.

* Correlazione temperatura–inquinamento
Grafico scatter che permette di individuare relazioni tra:

- temperatura  
- AQI  
- PM2.5  
- PM10  

---

📷 Interfaccia
L’interfaccia utilizza GunaUI2 per ottenere:

- pulsanti moderni  
- textbox stilizzate  
- pannelli con ombre e bordi arrotondati  
- transizioni fluide  

Lo sfondo cambia automaticamente in base al meteo (sole, pioggia, neve, nuvole).
