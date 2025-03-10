import os
import openai
from openai import OpenAI
from fastapi import FastAPI
from pydantic import BaseModel
from fastapi.responses import JSONResponse


app = FastAPI()
base_url = "https://api.aimlapi.com/v1"
api_key = "97d401ef2d974e1e9fea84852dec918b"

api = OpenAI(api_key=api_key, base_url=base_url)

class BattleMessage(BaseModel):
    name: str
    action: str #attack, defend, use item
    enemy: str #enemy name ()


    
 
@app.post("/battle-ai")
async def battle_ai(message: BattleMessage):
    prompt=f"""
    You are an AI battle guide in a fantasy RPG.
    The player is fighting a {message.enemy}.
    The player chooses to {message.action}.
    
    Provide a smart and engaging battle response.
    """
    #calls openai
 
    response = openai.chat.completions.create(
        model="gpt-4o", 
        messages=[{"role": "system", "content": "You are a battle AI for a game."},
                {"role": "user", "content": prompt}],
        temperature=0.7,
        max_tokens=200
    
    )
    battle_response = response["choices"][0]["message"]["content"]
    print(battle_response)
    return {"response": battle_response}
