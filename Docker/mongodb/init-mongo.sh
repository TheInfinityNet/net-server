#!/bin/bash

# Đảm bảo thư mục chứa file JSON đã có dữ liệu
if [ ! -d "/backup" ]; then
  echo "Directory /backup does not exist. Creating it..."
  mkdir -p /backup

# Đảm bảo thư mục chứa file JSON đã có dữ liệu
if [ -f /backup/Notifications.json ]; then
  echo "Importing data into MongoDB..."

  # Sử dụng mongoimport để import dữ liệu
  mongoimport --uri="mongodb://infinitynetUser:Password%40123@infinity-net-mongodb:27017" --db=notification1_db --collection=Notifications --file=/backup/Notifications.json --jsonArray

  echo "Data imported successfully"
else
  echo "No data to import"
fi
