import React, { useState, useEffect } from 'react';
import Command from './models/Command';
import 'bootstrap/dist/css/bootstrap.min.css';
const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;

const mic = new SpeechRecognition();
mic.continous = true;
mic.interimResults = true;
mic.lang = 'hu-HU';

let wakeWord = false;
let wakeWordOnCommand = false;
let pre = "";
let lowerPre = "";

let commandsFromDataModel = []
let commands = []

function App() {
  
  useEffect(() => {
    (async () => {
      await getdata();          
      const commands = commandsFromDataModel.map(command => command.CommandRec);
      console.log(commands);
    })();
  }, []);

  const [isListening, setIsListening] = useState('false');
  const [note, setNote] = useState(null);

  useEffect(() => {

    const handleListen = () => {
      if (isListening) {
        mic.start()
        mic.onend = () => {
          if (wakeWord && !wakeWordOnCommand) {
            console.log('become true')
            wakeWordOnCommand = true;
          }
          else if (wakeWordOnCommand) {
            console.log(`onend:${lowerPre}`)
            pre = lowerPre;
            let comm = findClosestMatch(pre)
            setNote(comm)
            if (comm != "nincs match") {              
              console.log(`LV: ${comm}`)              
              setNote(pre)              
              fetch('http://localhost:5025/Command', {
                method: 'POST',
                headers: {
                  'Content-Type': 'application/json'
                },                
                body: JSON.stringify(new Command(commandsFromDataModel.find(command => command.CommandRec == pre).CommandCode))
              })
                .then(response => response.json())
                .then(data => {
                  console.log('Response:', data);
                })
                .catch(error => {
                  console.error('Error:', error);
                });


              wakeWord = false;
              wakeWordOnCommand = false;
            }
            else {
              wakeWord = false;
              wakeWordOnCommand = false;
            }

          }
          mic.start()
        }
      } else {
        mic.stop()
        mic.onend = () => {
          console.log('Stopped Mic')
        }
      }
      mic.onstart = () => {
        console.log('Mics on')
      }

      mic.onresult = event => {
        const transcript = Array.from(event.results)
          .map(result => result[0])
          .map(result => result.transcript)
          .join('')
        console.log(transcript)
        let lowerTranscript = transcript.toLowerCase()
        lowerPre = lowerTranscript;
        if (!wakeWord) {
          if (lowerTranscript === 'ödön' || lowerTranscript === 'heyden' || lowerTranscript === 'hayden' || lowerTranscript === 'haiden' || lowerTranscript === 'heiden' || lowerTranscript === 'hilden') {
            setNote("hey Ödön");
            wakeWord = true;
            console.log("wakeword noticed")
          }
        }

        mic.onerror = event => {
          console.log(event.error)
        }
      }
    };

    handleListen();
  }, [isListening]);


  return (
    <>
      <h1>Voice Assistant</h1>
      <div className="divButttonText">
        <div className="box">         
          <p className='commandOnPage'>{note}</p>
        </div>
      </div>
    </>
  )
}

async function getdata() {
  await fetch('http://localhost:5025/Command')
    .then(response => response.json())
    .then(data => {
      commandsFromDataModel = data;
    })
    .catch(error => {
      console.error('Error fetching data:', error);
    });
}

function findClosestMatch(inputString) {
  let closestMatch = '';
  let closestDistance = Infinity;
  let index = -1;
  for (let i = 0; i < commands.length; i++) {
    const currentString = commands[i];
    const distance = calculateLevenshteinDistance(inputString, currentString);
    console.log(`in func:${inputString}`)
    if (distance < closestDistance) {
      closestDistance = distance;
      closestMatch = currentString;
      index = i;
    }
    if (closestDistance === 0) {      
      break;
    }
  }
  console.log('closest:' + closestMatch)
  return closestDistance <= 5 ? `${closestMatch}${index}` : "nincs match";
}

function calculateLevenshteinDistance(string1, string2) {
  const m = string1.length;
  const n = string2.length;

  const dp = new Array(m + 1).fill().map(() => new Array(n + 1).fill(0));

  for (let i = 0; i <= m; i++) {
    dp[i][0] = i;
  }

  for (let j = 0; j <= n; j++) {
    dp[0][j] = j;
  }

  for (let i = 1; i <= m; i++) {
    for (let j = 1; j <= n; j++) {
      if (string1[i - 1] === string2[j - 1]) {
        dp[i][j] = dp[i - 1][j - 1];
      } else {
        dp[i][j] =
          1 +
          Math.min(dp[i - 1][j], dp[i][j - 1], dp[i - 1][j - 1]);
      }
    }
  }
  return dp[m][n];
}

export default App