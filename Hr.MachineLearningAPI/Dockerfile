FROM python:3.10-slim

# 1 Variables de entorno (importante para OpenCV)
ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1

# 2 Dependencias del sistema (OpenCV + Paddle)
RUN apt-get update && apt-get install -y \
    libgl1 \
    libglib2.0-0 \
    libsm6 \
    libxext6 \
    libxrender1 \
    gcc \
    && rm -rf /var/lib/apt/lists/*

# 3 Directorio de trabajo
WORKDIR /app

# 4 Copiamos requirements primero (cache)
COPY requirements.txt .

# 5 Instalamos dependencias Python
RUN pip install --no-cache-dir --upgrade pip && \
    pip install --no-cache-dir -r requirements.txt

# 6 Copiamos el código
COPY . .

# 7 Pre-descargar modelos PaddleOCR (clave)
RUN python -c "from paddleocr import PaddleOCR; PaddleOCR(use_angle_cls=True, lang='en')"

# 8 Exponer puerto
EXPOSE 8035

# 9 Comando de arranque
CMD ["uvicorn", "Main:app", "--host", "0.0.0.0", "--port", "8035"]