import os
from fastapi import FastAPI
from pydantic import BaseModel
from openai import OpenAI
from fastapi.responses import JSONResponse

app = FastAPI()

# Claude bc i ran out of free calls from openai :( AI API endpoint and API key
base_url = "https://api.aimlapi.com/v1"
api_key = "97d401ef2d974e1e9fea84852dec918b"

# Initialize OpenAI API client with Claude's base URL and API key
api = OpenAI(api_key=api_key, base_url=base_url)

# Define the BattleMessage schema
class BattleMessage(BaseModel):
    name: str
    action: str  # attack, defend, use item
    enemy: str  # enemy name


@app.post("/battle-ai")
async def battle_ai(message: BattleMessage):
    # Prepare the prompt for Claude AI
    system_prompt = "You are an AI battle guide in a fantasy RPG."
    user_prompt = f"The player is fighting a {message.enemy}. The player chooses to {message.action}. Provide a smart and engaging battle response."
    
    try:
        # Call Claude AI API to generate a battle response
        completion = api.chat.completions.create(
            model="mistralai/Mistral-7B-Instruct-v0.2",  # Ensure you're using the correct model
            messages=[
                {"role": "system", "content": system_prompt},
                {"role": "user", "content": user_prompt}
            ],
            temperature=0.7,
            max_tokens=200
        )
        
        # Extract the response content from the AI's message
        battle_response = completion.choices[0].message.content
        print(battle_response)
        return {"response": battle_response}
    
    except Exception as e:
        # Handle any exceptions (e.g., API issues)
        return JSONResponse(status_code=500, content={"error": f"Request failed: {str(e)}"})
