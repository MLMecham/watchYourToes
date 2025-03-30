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
    Days : int


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
    print("you tried to talk to the villager")


    #TODOOO pass on the days it's been to villager
    user_prompt = "You are a shopkeeper in a medieval fantasy world. Your village has been cursed into a time loop until you retrieve the item malgar wants in the dungeon. You run a small shop that sells consumables. When a customer asks to shop, list the available items and their prices. If they ask to buy something, confirm the purchase. Be immersive and stay in character. Depenind on the amount of days past the beginning, you should start going crazy. Currently you are on day " + str(request.Days) + "Limit your response to 1 to 2 sentences."
    response = client.models.generate_content(
        model="gemini-2.0-flash",
        contents=[user_prompt, request.User_query]
    )
    return response.text


@app.get("/test")
async def test():
    return {"message": "Hello, World!"}


#OPEN AI
# import os
# from fastapi import FastAPI, HTTPException
# from pydantic import BaseModel
# from openai import OpenAI
# from dotenv import load_dotenv


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











# import os
# import google.generativeai as genai
# from fastapi import FastAPI, HTTPException
# from pydantic import BaseModel
# from fastapi.responses import JSONResponse
# from dotenv import load_dotenv


# load_dotenv()

# # Load the API key from the environment
# api_key = os.getenv("GEMINI_API_KEY")
# if not api_key:
#     raise ValueError("API key not found. Set GEMINI_API_KEY in your .env file.")

# # Initialize the Gemini client (without Client class)
# genai.configure(api_key=api_key)

# app = FastAPI()

# # Define the models
# class BattleMessage(BaseModel):
#     Name: str
#     Name_Class: str
#     Action: str
#     Target: str
#     Target_Class: str

# class VillagerMessage(BaseModel):
#     User_query: str
#     Days: int


# @app.post("/battle_chat")
# async def battle_ai(request: BattleMessage):
#     try:
#         # Input message for battle AI prompt
#         user_prompt = f"You are an AI battle guide in a fantasy RPG. {request.Name}, a {request.Name_Class}, performs {request.Action} on {request.Target}, a {request.Target_Class}. Provide a smart and engaging battle response. Limit it to 3 sentences."
        
#         # Make request to Gemini API
#         response = genai.generate_content(
#             model="gemini-2.0-flash",
#             contents=[user_prompt]
#         )

#         # Parse the response properly
#         if response and 'text' in response:
#             return response['text']
        
#         raise HTTPException(status_code=500, detail="Gemini AI returned no valid response.")

#     except Exception as e:
#         # Catch any errors and return an informative message
#         return JSONResponse(status_code=500, content={"error": f"Request failed: {str(e)}"})


# @app.post("/villager_chat")
# async def villager_chat(request: VillagerMessage):
#     try:
#         # Prepare the villager prompt based on days
#         user_prompt = f"You are a villager in a medieval fantasy world. Your village has been cursed into a time loop until you retrieve the item malgar wants in the dungeon. Depending on the amount of days past the beginning, you should start going crazy. Currently, you are on day {request.Days}. Answer questions in a helpful and immersive way. Limit your response to 1 to 2 sentences."
        
#         # Call the Gemini API for response
#         response = genai.generate_content(
#             model="gemini-2.0-flash",
#             contents=[user_prompt, request.User_query]
#         )

#         # Parse and return the response text
#         if response and 'text' in response:
#             return response['text']

#         raise HTTPException(status_code=500, detail="Gemini AI returned no valid response.")

#     except Exception as e:
#         # Handle any errors
#         return JSONResponse(status_code=500, content={"error": f"Request failed: {str(e)}"})
