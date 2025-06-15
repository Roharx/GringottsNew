FROM node:18-alpine
WORKDIR /usr/src/app
COPY src ./
RUN npm install -g http-server
CMD ["http-server", ".", "-p", "4200"]
