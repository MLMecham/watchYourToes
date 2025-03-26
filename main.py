#Gemini
import os
from google import genai
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from fastapi.responses import JSONResponse
from dotenv import load_dotenv


load_dotenv()


api_key = os.getenv("GEMINI_API_KEY")  
if not api_key:
    raise ValueError("API key not found. Set GEMINI_API_KEY in your .env file.")


client = genai.Client(api_key=api_key)  


app = FastAPI()

class BattleMessage(BaseModel):
    Name: str 
    Name_Class: str
    Action: str
    Target: str 
    Target_Class: str

class VillagerMessage(BaseModel):
    User_query: str


@app.post("/battle_chat")
async def battle_ai(request: BattleMessage):
    try:
        #input message
        user_prompt = f"You are an AI battle guide in a fantasy RPG. {request.Name}, a {request.Name_Class}, performs {request.Action} on {request.Target}, a {request.Target_Class}. Provide a smart and engaging battle response. Limit it to 3 sentences."        
        response = client.models.generate_content(
            model="gemini-2.0-flash",  
            contents=[user_prompt]     
        )
        if not response.text:
            raise HTTPException(status_code=500, detail="Gemini AI returned no response.")

        return response.text

    except Exception as e:
        return JSONResponse(status_code=500, content={"error": f"Request failed: {str(e)}"})
    
@app.post("/villager_chat")
async def villager_chat(request: VillagerMessage):
    user_prompt = "You are a villager in a medieval fantasy world. Answer questions in a helpful and immersive way. Limit your response to 1 to 2 sentences."
    response = client.models.generate_content(
        model="gemini-2.0-flash",
        contents=[user_prompt, request.User_query]
    )
    return response.text


#OPEN AI
# import os
# from fastapi import FastAPI, HTTPException
# from pydantic import BaseModel
# from openai import OpenAI
# from dotenv import load_dotenv

# load_dotenv()
# api_key = os.getenv("OPENAI_API_KEY")
# if not api_key:
#     raise ValueError("API key not found. Set OPENAI_API_KEY in your .env file.")

# client = OpenAI(api_key=api_key)

# app = FastAPI()

# #class BattleMessage(BaseModel):
#     Name: str 
#     Name_Class: str
#     Action: str
#     Target: str 
# #     Target_Class: str

# class VillagerMessage(BaseModel):
#     User_query: str

# @app.post("/battle_chat")
# async def battle_chat(request: BattleMessage):
#     user_prompt = f"You are an AI battle guide in a fantasy RPG. {request.Name}, a {request.Name_Class}, performs {request.Action} on {request.Target}, a {request.Target_Class}. Provide a smart and engaging battle response. Limit it to 3 sentences."        
#     response = client.chat.completions.create(
#         model="gpt-4o-mini",
#         messages=[{"role": "user", "content": user_prompt}]
#     )
#     return response.choices[0].message.content

# @app.post("/villager_chat")
# async def villager_chat(request: VillagerMessage):
#     user_prompt = "You are a villager in a medieval fantasy world. Answer questions in a helpful and immersive way. Limit your response to 1 to 2 sentences."
#     response = client.chat.completions.create(
#         model="gpt-4o-mini",
#         messages=[{"role": "user", "content": user_prompt}]
#     )
#     # return {"response": response.choices[0].message.content}