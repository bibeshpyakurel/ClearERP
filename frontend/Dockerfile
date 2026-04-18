# Stage 1: Build
FROM node:22-alpine AS builder
WORKDIR /app

ARG VITE_API_BASE_URL=http://localhost:5000/api
ARG VITE_APP_NAME=ClearERP
ARG VITE_AUTH_TOKEN_STORAGE_KEY=clear-erp.access-token

ENV VITE_API_BASE_URL=$VITE_API_BASE_URL
ENV VITE_APP_NAME=$VITE_APP_NAME
ENV VITE_AUTH_TOKEN_STORAGE_KEY=$VITE_AUTH_TOKEN_STORAGE_KEY

COPY frontend/package*.json ./
RUN npm ci
COPY frontend ./
RUN npm run build

# Stage 2: Serve with nginx
FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html
COPY frontend/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
