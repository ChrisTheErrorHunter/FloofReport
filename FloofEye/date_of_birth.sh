#!/bin/bash

stat $1 | tail -1 | cut -c 9- | rev | cut -c 11- | rev | tr -d '\n';
