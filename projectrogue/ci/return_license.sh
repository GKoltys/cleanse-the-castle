#!/usr/bin/env bash

set -euo pipefail

if [[ -n "${UNITY_LICENSING_SERVER:-}" ]]; then
  if [[ -z "${FLOATING_LICENSE:-}" ]]; then
    echo "Warning: FLOATING_LICENSE environment variable is not set" >&2
    exit 0
  fi

  echo "Returning floating license: \"$FLOATING_LICENSE\""
  if ! /opt/unity/Editor/Data/Resources/Licensing/Client/Unity.Licensing.Client --return-floating "$FLOATING_LICENSE"; then
    echo "Warning: Failed to return floating license" >&2
    exit 0
  fi
elif [[ -n "${UNITY_SERIAL:-}" ]]; then
  # Validate required environment variables
  for var in UNITY_EMAIL UNITY_PASSWORD; do
    if [[ -z "${!var:-}" ]]; then
      echo "Warning: $var environment variable is not set" >&2
      exit 0
    fi
  done

  echo "Returning serial license for user: $UNITY_EMAIL"
  project_path="../unity-builder/dist/BlankProject"
  if [[ ! -d "$project_path" ]]; then
    echo "Warning: Project path not found. Ensure before_script.sh was executed successfully" >&2
    exit 0
  fi

  unity-editor \
    -logFile /dev/stdout \
    -quit \
    -returnlicense \
    -username "$UNITY_EMAIL" \
    -password "$UNITY_PASSWORD" \
    -projectPath "$project_path" || {
      echo "Warning: Failed to return serial license" >&2
      exit 0
    }
else
  echo "Warning: Neither UNITY_LICENSING_SERVER nor UNITY_SERIAL is set" >&2
  exit 0
fi
